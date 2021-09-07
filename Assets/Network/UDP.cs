using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

internal class UDP
{
    private Socket socket;
    private EndPoint serverEndPoint;
    public EndPoint localEndPoint;
    private CancellationTokenSource cancellationTokenSource;

    public void Connect(EndPoint ServerEndPoint)
    {
        cancellationTokenSource = new CancellationTokenSource();
        try
        {
            serverEndPoint = ServerEndPoint;
            socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEndPoint);
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
                byte[] udpBuffer = ArrayPool<byte>.Shared.Rent(Constants.MAX_BUFFER_SIZE);
                SocketReceiveFromResult socketData = await SocketTaskExtensions.ReceiveFromAsync(socket, new ArraySegment<byte>(udpBuffer), SocketFlags.None, localEndPoint).ConfigureAwait(false);
                if (socketData.ReceivedBytes < 4)
                {
                    continue;
                }
                byte[] received_buffer = ArrayPool<byte>.Shared.Rent(socketData.ReceivedBytes);
                Array.Copy(udpBuffer, received_buffer, socketData.ReceivedBytes);
                ArrayPool<byte>.Shared.Return(udpBuffer);
                HandleData(udpBuffer);
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
            _ = SocketTaskExtensions.SendToAsync(socket, new ArraySegment<byte>(packet.ToArray()), SocketFlags.None, serverEndPoint).ConfigureAwait(false);
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
