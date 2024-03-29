﻿using System.Net.Sockets;

namespace Cl_Homework_3
{
    // Структурируйте код клиента и сервера чата, используя знания о шаблонах.
    public class Client
    {
        static async Task Main(string[] args)
        {
            using TcpClient tcpClient = new TcpClient();
            Console.Write("Введите свое имя: ");
            string? userName = Console.ReadLine();
            Thread.Sleep(1000);

            try
            {
                tcpClient.Connect("127.0.0.1", 12345);
                Console.WriteLine($"Добро пожаловать, {userName}");

                var reader = new StreamReader(tcpClient.GetStream());
                var writer = new StreamWriter(tcpClient.GetStream());

                if (writer is null || reader is null) return;

                Task.Run(() => ReceiveMessage(reader));
                await SendMessage(writer);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            finally { tcpClient?.Close(); }

            async Task SendMessage(StreamWriter writer)
            {
                await writer.WriteLineAsync(userName);
                await writer.FlushAsync();

                Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");
                Console.WriteLine("Для выхода из чата введите Exit");

                string? message = "";

                while (true)
                {
                    if (message != "exit" && message != "cancel" && tcpClient.Connected)
                    {
                        message = Console.ReadLine().ToLower();
                        await writer.WriteLineAsync(message);
                        await writer.FlushAsync();
                    }
                    else break;
                }
            }

            async Task ReceiveMessage(StreamReader reader)
            {
                while (reader != null)
                {
                    string? message = await reader.ReadLineAsync();
                    Console.WriteLine(message);
                }
            }
        }
    }
}
