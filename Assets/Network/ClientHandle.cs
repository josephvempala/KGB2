
using System;
using System.Net;
using UnityEngine;

internal static class ClientHandle
{
    public static void Welcome(Packet packet)
    {
        Client.instance.id = packet.ReadInt();
        string item = packet.ReadString();
        Debug.Log($"Server says {item}");
        ClientSend.WelcomeReceived();
        Client.instance.udp.localEndPoint = Client.instance.tcp.LocalEndPoint as IPEndPoint;
        Client.instance.udp.Connect(Client.instance.ServerEndpoint);
    }
    public static void Message(Packet packet)
    {
        int client_id = packet.ReadInt();
        string item = packet.ReadString();
        Debug.Log($"Client {client_id} says through UDP : {item}");
    }

    public static void SpawnPlayer(Packet packet)
    {
        int id = packet.ReadInt();
        string username = packet.ReadString();
        Vector3 vector3 = packet.ReadVector3();
        Quaternion rotation = packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(id, username, vector3, rotation);
    }

    public static void PlayerState(Packet packet)
    {
        int client_id = packet.ReadInt();
        uint tick = (uint)packet.ReadLong();
        ClientState newState;
        newState.position = packet.ReadVector3();
        newState.rotation = packet.ReadQuaternion();

        GameManager.players[client_id].RecieveMovement(newState, tick);
    }
}
