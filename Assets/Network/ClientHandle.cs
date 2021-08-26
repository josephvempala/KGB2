
using System;


internal static class ClientHandle
{
    public static void Welcome(Packet packet)
    {
        Client.instance.id = packet.ReadInt();
        string item = packet.ReadString();
        Console.WriteLine($"Server says {item}");
        ClientSend.WelcomeReceived("thanq for welcome");
        Client.instance.udp.localEndPoint = Client.instance.tcp.LocalEndPoint;
        Client.instance.udp.Connect(Client.instance.ServerEndpoint);
    }
    public static void Message(Packet packet)
    {
        int client_id = packet.ReadInt();
        string item = packet.ReadString();
        Console.WriteLine($"Client {client_id} says through UDP : {item}");
    }
}
