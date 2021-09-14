﻿
using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

internal class TCP
{
    private NetworkStream stream;
    private Packet receivedData;
    private Socket socket;
    public EndPoint LocalEndPoint;
    private CancellationTokenSource cancellationTokenSource;

    public void Connect(EndPoint endpoint)
    {
        cancellationTokenSource = new CancellationTokenSource();
        try
        {
            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endpoint);
            LocalEndPoint = socket.LocalEndPoint;
            stream = new NetworkStream(socket);
        }
        catch (Exception ex)
        {
            Debug.Log($"Exception encountered while connecting to TCP server {ex}");
            return;
        }
        _ = Task.Run(() => Receive(cancellationTokenSource.Token));
    }

    public void Send(Packet packet)
    {
        if (socket == null)
        {
            Debug.Log($"Call Connect(IPEndpoint ep) on the TCP object with an IPEndpoint as parameter before calling Send(Packet p)");
            return;
        }
        try
        {
            byte[] buffer = packet.ToArray();
            _ = stream.WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Debug.Log($"Exception encountered when Sending TCP packet {ex}");
        }
    }

    private async Task Receive(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                receivedData = new Packet();
                byte[] receiveBuffer = ArrayPool<byte>.Shared.Rent(Constants.MAX_BUFFER_SIZE);
                int bytes_read = await stream.ReadAsync(receiveBuffer, 0, Constants.MAX_BUFFER_SIZE).ConfigureAwait(false);
                if (bytes_read == 0)
                {
                    Client.instance.Disconnect();
                    continue;
                }

                byte[] data_read = ArrayPool<byte>.Shared.Rent(bytes_read);
                Array.Copy(receiveBuffer, data_read, bytes_read);
                ArrayPool<byte>.Shared.Return(receiveBuffer);
                receivedData.Reset(HandleData(data_read));
            }
            catch (Exception ex)
            {
                Disconnect();
                Debug.Log($"Exception encountered when Receiving TCP packet {ex}");
            }
        }
    }

    public void Disconnect()
    {
        cancellationTokenSource.Cancel();
        stream.Close();
        socket.Close();
        receivedData.Dispose();
        cancellationTokenSource.Dispose();
    }

    private bool HandleData(byte[] data)
    {
        int packet_length = 0;
        receivedData.SetBytes(data);

        if (receivedData.UnreadLength >= 4)
        {
            packet_length = receivedData.ReadInt();
            if (packet_length == 0)
            {
                ArrayPool<byte>.Shared.Return(data);
                return true;
            }
        }
        while (packet_length > 0 && packet_length <= receivedData.UnreadLength)
        {
            byte[] packet_Bytes = receivedData.ReadBytes(packet_length);
            Packet packet = new Packet(packet_Bytes);
            int packet_id = packet.ReadInt();
            if (Client.instance.packetHandlers.ContainsKey(packet_id))
            {
                TickManager.ExecuteOnTick(() =>
                {
                    Client.instance.packetHandlers[packet_id].Invoke(packet);
                    packet.Dispose();
                });
            }

            packet_length = 0;
            if (receivedData.UnreadLength >= 4)
            {
                packet_length = receivedData.ReadInt();
                if (packet_length == 0)
                {
                    ArrayPool<byte>.Shared.Return(data);
                    return true;
                }
            }
        }
        if (packet_length <= 1)
        {
            ArrayPool<byte>.Shared.Return(data);
            return true;
        }
        return false;
    }
}