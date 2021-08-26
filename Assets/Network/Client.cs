﻿using System.Collections.Generic;
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

    public void Awake()
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

    public void Connect(IPEndPoint endPoint)
    {
        ServerEndpoint = endPoint;
        InitializePacketHandlers();
        tcp.Connect(ServerEndpoint);
    }

    public void Disconnect()
    {
        tcp.Disconnect();
        udp.Disconnect();
    }

    private void InitializePacketHandlers()
    {
        packetHandlers = new Dictionary<int, PacketHandler>
        {
            {(int)ServerPackets.welcome, ClientHandle.Welcome },
            {(int)ServerPackets.message, ClientHandle.Message }
        };
    }
}
