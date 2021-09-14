
using System;
using UnityEngine;

internal static class ClientHandle
{
    public static void Welcome(Packet packet)
    {
        Client.instance.id = packet.ReadInt();
        string item = packet.ReadString();
        Debug.Log($"Server says {item}");
        ClientSend.WelcomeReceived();
        Client.instance.udp.localEndPoint = Client.instance.tcp.LocalEndPoint;
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

    public static void PlayerRotation(Packet packet)
    {
        int playerid = packet.ReadInt();
        Quaternion playerRotation = packet.ReadQuaternion();
        
        GameManager.players[playerid].transform.rotation = playerRotation;
    }

    public static void PlayerPosition(Packet packet)
    {
        int playerid = packet.ReadInt();
        Vector3 playerPosition = packet.ReadVector3();

        GameManager.players[playerid].transform.position = playerPosition;
    }
}
