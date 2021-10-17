internal static class ClientSend
{
    private static void SendTcpData(Packet packet)
    {
        packet.WriteLength();
        Client.Instance.Tcp.Send(packet);
    }

    private static void SendUdpData(Packet packet)
    {
        Client.Instance.Udp.Send(packet);
    }

    public static void WelcomeReceived()
    {
        using var packet = new Packet((int) ClientPackets.WelcomeReceived);
        packet.Write(UIManager.Instance.usernameField.text);
        SendTcpData(packet);
    }

    public static void SendMessage(string message)
    {
        using var packet = new Packet((int) ClientPackets.Message);
        packet.Write(message);
        SendUdpData(packet);
    }

    public static void SendControls(byte[] controls)
    {
        using var packet = new Packet((int) ClientPackets.PlayerControls);
        packet.Write(controls.Length);
        packet.Write(controls);
        SendUdpData(packet);
    }

    public static void SendOrientation(byte[] orientation)
    {
        using var packet = new Packet((int) ClientPackets.PlayerOrientation);
        packet.Write(orientation.Length);
        packet.Write(orientation);
        SendUdpData(packet);
    }
}