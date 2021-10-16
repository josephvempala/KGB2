using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

internal class TCP
{
    private CancellationTokenSource _cancellationTokenSource;
    public EndPoint LocalEndPoint;
    private Packet _receivedData;
    private Socket _socket;
    private NetworkStream _stream;

    public void Connect(EndPoint endpoint)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        try
        {
            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(endpoint);
            LocalEndPoint = _socket.LocalEndPoint;
            _stream = new NetworkStream(_socket);
        }
        catch (Exception ex)
        {
            Debug.Log($"Exception encountered while connecting to TCP server {ex}");
            return;
        }

        _ = Task.Run(() => Receive(_cancellationTokenSource.Token));
    }

    public void Send(Packet packet)
    {
        if (_socket == null)
        {
            Debug.Log(
                "Call Connect(IPEndpoint ep) on the TCP object with an IPEndpoint as parameter before calling Send(Packet p)");
            return;
        }

        try
        {
            var buffer = packet.ToArray();
            _ = _stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.Log($"Exception encountered when Sending TCP packet {ex}");
        }
    }

    private async Task Receive(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
            try
            {
                _receivedData = new Packet();
                var receiveBuffer = ArrayPool<byte>.Shared.Rent(Constants.MAXBufferSize);
                var bytesRead = await _stream.ReadAsync(receiveBuffer, 0, Constants.MAXBufferSize, token)
                    .ConfigureAwait(false);
                if (bytesRead == 0)
                {
                    Client.Instance.Disconnect();
                    continue;
                }

                var dataRead = ArrayPool<byte>.Shared.Rent(bytesRead);
                Array.Copy(receiveBuffer, dataRead, bytesRead);
                ArrayPool<byte>.Shared.Return(receiveBuffer);
                _receivedData.Reset(HandleData(dataRead));
            }
            catch (Exception ex)
            {
                Disconnect();
                Debug.Log($"Exception encountered when Receiving TCP packet {ex}");
            }
    }

    public void Disconnect()
    {
        _cancellationTokenSource.Cancel();
        _stream.Close();
        _socket.Close();
        _receivedData.Dispose();
        _cancellationTokenSource.Dispose();
    }

    private bool HandleData(byte[] data)
    {
        var packetLength = 0;
        _receivedData.SetBytes(data);

        if (_receivedData.UnreadLength >= 4)
        {
            packetLength = _receivedData.ReadInt();
            if (packetLength == 0)
            {
                ArrayPool<byte>.Shared.Return(data);
                return true;
            }
        }

        while (packetLength > 0 && packetLength <= _receivedData.UnreadLength)
        {
            var packetBytes = _receivedData.ReadBytes(packetLength);
            var packet = new Packet(packetBytes);
            var packetID = packet.ReadInt();
            if (Client.Instance.PacketHandlers.ContainsKey(packetID))
                TickManager.ExecuteOnTick(() =>
                {
                    Client.Instance.PacketHandlers[packetID].Invoke(packet);
                    packet.Dispose();
                });

            packetLength = 0;
            if (_receivedData.UnreadLength < 4) continue;
            packetLength = _receivedData.ReadInt();
            if (packetLength != 0) continue;
            ArrayPool<byte>.Shared.Return(data);
            return true;
        }

        if (packetLength <= 1)
        {
            ArrayPool<byte>.Shared.Return(data);
            return true;
        }

        return false;
    }
}