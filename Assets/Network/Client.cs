using System.Collections.Generic;
using System.Net;
using UnityEngine;

internal class Client : MonoBehaviour
{
    public delegate void PacketHandler(Packet packet);
    public Dictionary<int, PacketHandler> packetHandlers;
    public IPEndPoint ServerEndpoint;
    public TCP tcp = new TCP();
    public UDP udp = new UDP();
    public int id;
    public static Client instance;
    private bool isConnected = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Connect(IPEndPoint endPoint)
    {
        ServerEndpoint = endPoint;
        InitializePacketHandlers();
        tcp.Connect(ServerEndpoint);
    }

    public void Disconnect()
    {
        if (!isConnected)
        {
            return;
        }
        isConnected = false;
        tcp.Disconnect();
        udp.Disconnect();
        Debug.Log("disconnected from server");
    }

    private void InitializePacketHandlers()
    {
        packetHandlers = new Dictionary<int, PacketHandler>
        {
            {(int)ServerPackets.welcome, ClientHandle.Welcome },
            {(int)ServerPackets.message, ClientHandle.Message },
            {(int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer  }
        };
    }
}
