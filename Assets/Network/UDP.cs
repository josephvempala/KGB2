using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

internal class UDP
{
    private UdpClient socket;
    private IPEndPoint serverEndPoint;
    public IPEndPoint localEndPoint;
    private CancellationTokenSource cancellationTokenSource;

    public void Connect(IPEndPoint ServerEndPoint)
    {
        cancellationTokenSource = new CancellationTokenSource();
        try
        {
            serverEndPoint = ServerEndPoint;
            socket = new UdpClient(localEndPoint);
            socket.Connect(serverEndPoint);
            using (Packet packet = new Packet())
            {
                Send(packet);
            }
        }
        catch (Exception ex)
        {
            Debug.Log($"Exception encountered while connecting to UDP server {ex}");
        }
        _ = Task.Run(() => Receive(cancellationTokenSource.Token));
    }


    private async Task Receive(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            try
            {
                var socketData = await socket.ReceiveAsync().ConfigureAwait(false);
                if (socketData.Buffer.Length < 4)
                {
                    continue;
                }
                byte[] received_buffer = ArrayPool<byte>.Shared.Rent(socketData.Buffer.Length);
                Array.Copy(socketData.Buffer, received_buffer, socketData.Buffer.Length);
                HandleData(received_buffer);
            }
            catch (Exception ex)
            {
                Debug.Log($"Exception encountered when Receiving UDP packet {ex}");
                Disconnect();
                return;
            }
        }
    }

    public void Send(Packet packet)
    {
        if (socket == null)
        {
            Debug.Log($"Call Connect(IPEndpoint ep) on the UDP object with an IPEndpoint as parameter before calling Send(Packet p)");
            return;
        }
        try
        {
            packet.InsertInt(Client.instance.id);
            _ = socket.SendAsync(packet.ToArray(),packet.Length).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Disconnect();
            Debug.Log($"Exception encountered when Sending UDP packet {ex}");
        }

    }

    public void Disconnect()
    {
        cancellationTokenSource.Cancel();
        socket.Close();
        localEndPoint = null;
    }

    private void HandleData(byte[] data)
    {
        Packet packet = new Packet(data);
        int packetId = packet.ReadInt();
        if (Client.instance.packetHandlers.ContainsKey(packetId))
        {
            TickManager.ExecuteOnTick(() =>
            {
                Client.instance.packetHandlers[packetId].Invoke(packet);
                ArrayPool<byte>.Shared.Return(data);
                packet.Dispose();
            });
        }
    }
}
