﻿using System;
using System.Net;
using System.Threading.Tasks;


internal static class Program
{
    public static bool isRunning = false;
    private static void Main(string[] args)
    {
        IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(args[0]), int.Parse(args[1]));
        Client.instance.Connect(endpoint);
        isRunning = true;
        _ = Task.Run(StartUpdateTicks);
        while (true)
        {
            string message = Console.ReadLine();
            ClientSend.SendMessage(message);
        }
    }
    public static async Task StartUpdateTicks()
    {
        Console.WriteLine("started client ticks");
        DateTime nextloop = DateTime.Now;
        while (isRunning)
        {
            while (nextloop < DateTime.Now)
            {
                TickManager.StartTick();

                nextloop = nextloop.AddMilliseconds(1000 / 30);

                if (nextloop > DateTime.Now)
                {
                    await Task.Delay(nextloop - DateTime.Now).ConfigureAwait(false);
                }
            }
        }
    }
}

