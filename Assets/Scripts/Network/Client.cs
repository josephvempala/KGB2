using System.Collections.Generic;
using System.Net;
using UnityEngine;

internal class Client : MonoBehaviour
{
    public delegate void PacketHandler(Packet packet);

    public static Client Instance;
    public int id;
    public Dictionary<int, PacketHandler> PacketHandlers;
    public IPEndPoint ServerEndpoint;
    public readonly TCP Tcp = new TCP();
    public readonly UDP Udp = new UDP();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) Destroy(this);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Connect(IPEndPoint endPoint)
    {
        ServerEndpoint = endPoint;
        InitializePacketHandlers();
        Tcp.Connect(ServerEndpoint);
    }

    public void Disconnect()
    {
        Tcp.Disconnect();
        Udp.Disconnect();
        Debug.Log("disconnected from server");
    }

    private void InitializePacketHandlers()
    {
        PacketHandlers = new Dictionary<int, PacketHandler>
        {
            {(int) ServerPackets.Welcome, ClientHandle.Welcome},
            {(int) ServerPackets.Message, ClientHandle.Message},
            {(int) ServerPackets.SpawnPlayer, ClientHandle.SpawnPlayer},
            {(int) ServerPackets.PlayerState, ClientHandle.PlayerState}
        };
    }
}