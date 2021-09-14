

using UnityEngine;

internal static class ClientSend
{
    private static void SendTCPData(Packet packet)
    {
        packet.WriteLength();
        Client.instance.tcp.Send(packet);
    }

    private static void SendUDPData(Packet packet)
    {
        Client.instance.udp.Send(packet);
    }

    public static void WelcomeReceived()
    {
        using (Packet packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            packet.Write(UIManager.instance.usernameField.text);
            SendTCPData(packet);
        }
    }
    public static void SendMessage(string message)
    {
        using (Packet packet = new Packet((int)ClientPackets.message))
        {
            packet.Write(message);
            SendUDPData(packet);
        }
    }
    public static void SendControls(byte[] controls)
    {
        using (Packet packet = new Packet((int)ClientPackets.playerControls))
        {
            packet.Write(controls.Length);
            packet.Write(controls);
            SendUDPData(packet);
        }
    }
}
