using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace Ser_Homework_2
{
    //Добавьте возможность ввести слово Exit в чате клиента, чтобы можно было завершить его работу.
    //В коде сервера добавьте ожидание нажатия клавиши, чтобы также прекратить его работу.
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
                    Task.Run(() => ServerOff(clients));

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
            finally { listener.Stop(); }
        }

        private async Task ProcessClient(TcpClient tcpClient, List<TcpClient> clients)
        {
            try
            {
                var reader = new StreamReader(tcpClient.GetStream());

                string? userName = await reader.ReadLineAsync();
                string? message = "";

                await Broadcast(clients, $"{userName} вошел в чат");

                Console.WriteLine(message);

                while (true)
                {
                    message = await reader.ReadLineAsync();

                    Console.WriteLine($"{userName}: {message}\n*****");

                    if (message == "exit") break;

                    await Broadcast(clients, $"{userName}: {message}");
                }
                await Broadcast(clients, $"{userName}: Вышел из чата");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
            }
            finally { DeleteUser(tcpClient, clients); }
        }

        private void DeleteUser(TcpClient tcpClient, List<TcpClient> clients)
        {
            clients.Remove(tcpClient);
            tcpClient?.Close();
        }

        private async Task Broadcast(List<TcpClient> clients, string formatMessage)
        {
            foreach (var client in clients)
            {
                var writer = new StreamWriter(client.GetStream());
                await writer.WriteLineAsync(formatMessage);
                await writer.FlushAsync();
            }
        }

        private async Task ServerOff(List<TcpClient> clients)
        {
            string off = await Console.In.ReadLineAsync();

            if (off == "q")
            {
                foreach (var client in clients)
                {
                    client.Close();
                }
                listener.Stop();
            }
        }
    }
}