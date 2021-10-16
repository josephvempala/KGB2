using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

internal class UDP
{
    private CancellationTokenSource _cancellationTokenSource;
    public IPEndPoint LocalEndPoint;
    private IPEndPoint _serverEndPoint;
    private UdpClient _socket;

    public void Connect(IPEndPoint serverEndPoint)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        try
        {
            _serverEndPoint = serverEndPoint;
            _socket = new UdpClient(LocalEndPoint);
            _socket.Connect(_serverEndPoint);
            using (var packet = new Packet())
            {
                Send(packet);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Exception encountered while connecting to UDP server {ex}");
        }

        _ = Task.Run(() => Receive(_cancellationTokenSource.Token));
    }


    private async Task Receive(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
            try
            {
                var socketData = await _socket.ReceiveAsync().ConfigureAwait(false);
                if (socketData.Buffer.Length < 4) continue;
                var receivedBuffer = ArrayPool<byte>.Shared.Rent(socketData.Buffer.Length);
                Array.Copy(socketData.Buffer, receivedBuffer, socketData.Buffer.Length);
                HandleData(receivedBuffer);
            }
            catch (Exception ex)
            {
                Debug.Log($"Exception encountered when Receiving UDP packet {ex}");
                Disconnect();
                return;
            }
    }

    public void Send(Packet packet)
    {
        if (_socket == null)
        {
            Debug.Log(
                "Call Connect(IPEndpoint ep) on the UDP object with an IPEndpoint as parameter before calling Send(Packet p)");
            return;
        }

        try
        {
            packet.InsertInt(Client.Instance.id);
            _ = _socket.SendAsync(packet.ToArray(), packet.Length).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Disconnect();
            Debug.Log($"Exception encountered when Sending UDP packet {ex}");
        }
    }

    public void Disconnect()
    {
        _cancellationTokenSource.Cancel();
        _socket.Close();
        LocalEndPoint = null;
    }

    private void HandleData(byte[] data)
    {
        var packet = new Packet(data);
        var packetId = packet.ReadInt();
        if (Client.Instance.PacketHandlers.ContainsKey(packetId))
            TickManager.ExecuteOnTick(() =>
            {
                Client.Instance.PacketHandlers[packetId].Invoke(packet);
                ArrayPool<byte>.Shared.Return(data);
                packet.Dispose();
            });
    }
}