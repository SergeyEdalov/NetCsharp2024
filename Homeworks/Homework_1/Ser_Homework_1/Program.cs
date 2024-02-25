using System.Net;
using System.Net.Sockets;

namespace Ser_Homework_1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var server = new ChatServer();
            await server.Run();
        }
    }


    public class ChatServer
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 12345);
        List<TcpClient> clients = new List<TcpClient>();

        public async Task Run()
        {
            try
            {
                listener.Start();
                Console.Out.WriteLine("Сервер запущен");

                while (true)
                {
                    var tcpClient = await listener.AcceptTcpClientAsync();

                    Console.WriteLine("Успешно подключен");

                    clients.Add(tcpClient);

                    Task.Run(() => ProcessClient(tcpClient, clients));

                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message.ToString());
            }
        }

        public async Task ProcessClient(TcpClient tcpClient, List<TcpClient> clients)
        {
            try
            {
                var reader = new StreamReader(tcpClient.GetStream());
                string? userName = await reader.ReadLineAsync();
                string? message = $"{userName} вошел в чат";
                //string? message = await reader.ReadLineAsync();
                // посылаем сообщение о входе в чат всем подключенным пользователям
                foreach (var client in clients)
                {
                    var writer = new StreamWriter(client.GetStream());
                    await writer.WriteLineAsync(message);
                    await writer.FlushAsync();
                }
                Console.WriteLine(message);

                while (true)
                {
                    //var reader = new StreamReader(tcpClient.GetStream());

                    message = await reader.ReadLineAsync();

                    Console.WriteLine($"{userName}: {message}");
                    Console.WriteLine("*****");

                    foreach (var client in clients)
                    {
                        var writer = new StreamWriter(client.GetStream());
                        await writer.WriteLineAsync($"{userName}: {message}");
                        await writer.FlushAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
        }
    }
}