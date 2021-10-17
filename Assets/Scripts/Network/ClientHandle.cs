using System.Net;
using UnityEngine;

internal static class ClientHandle
{
    public static void Welcome(Packet packet)
    {
        Client.Instance.id = packet.ReadInt();
        var item = packet.ReadString();
        Debug.Log($"Server says {item}");
        ClientSend.WelcomeReceived();
        Client.Instance.Udp.LocalEndPoint = Client.Instance.Tcp.LocalEndPoint as IPEndPoint;
        Client.Instance.Udp.Connect(Client.Instance.ServerEndpoint);
    }

    public static void Message(Packet packet)
    {
        var clientID = packet.ReadInt();
        var item = packet.ReadString();
        Debug.Log($"Client {clientID.ToString()} says through UDP : {item}");
    }

    public static void SpawnPlayer(Packet packet)
    {
        var id = packet.ReadInt();
        var username = packet.ReadString();
        var vector3 = packet.ReadVector3();
        var rotation = packet.ReadQuaternion();

        GameManager.Instance.SpawnPlayer(id, username, vector3, rotation);
    }

    public static void PlayerState(Packet packet)
    {
        var clientID = packet.ReadInt();
        var tick = (uint) packet.ReadLong();
        ClientState newState;
        newState.Position = packet.ReadVector3();
        newState.Rotation = packet.ReadQuaternion();

        GameManager.Players[clientID].ReceiveMovement(newState, tick);
    }
}