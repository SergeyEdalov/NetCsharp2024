using System.Net;
using System.Net.Sockets;

namespace Ser_Homework_3
{
    // Структурируйте код клиента и сервера чата, используя знания о шаблонах.
    public class Program
    {
        static async Task Main(string[] args)
        {
            await ChatServer.Instance.Run(); //realize singleton
        }
    }

    public class ChatServer
    {

        private static ChatServer instance;
        private static readonly object lockObject = new object();
        
        static private CancellationTokenSource cts = new CancellationTokenSource();
        static private CancellationToken ct = cts.Token;

        TcpListener listener = new TcpListener(IPAddress.Any, 12345);
        List<TcpClient> clients = new List<TcpClient>();

        private ChatServer() { }

        public static ChatServer Instance
        {
            get 
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new ChatServer();
                        }
                    }
                } return instance;
            }
        }

        public async Task Run()
        {
            try
            {
                listener.Start();
                Console.Out.WriteLine("Сервер запущен");

                while (true)
                {
                    Task.Run(() => ServerOff(clients), ct);

                    var tcpClient = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("Успешно подключен");
                    clients.Add(tcpClient);

                    Task.Run(() => ProcessClient(tcpClient, clients), ct);

                    if (cts.IsCancellationRequested)
                    {
                        ct.ThrowIfCancellationRequested();
                        break;
                    }
                }
            }
            catch (OperationCanceledException) { Console.WriteLine("Операция прервана со стороны сервера"); }
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

                    if (message == "exit") { break; }
                    if (message == "cancel") { ServerOrUserStop(); break; }

                    await Broadcast(clients, $"{userName}: {message}");
                }
                if (ct.IsCancellationRequested) { ct.ThrowIfCancellationRequested(); }

                await Broadcast(clients, $"{userName}: Вышел из чата");
            }
            catch (OperationCanceledException) { Console.WriteLine("Операция прервана со стороны пользователя"); }
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
            string? off = await Console.In.ReadLineAsync();

            if (off == "q")
            {
                foreach (var client in clients)
                {
                    client.Close();
                }
                listener.Stop();
            }
            else if (off == "stop") { ServerOrUserStop(); }
        }

        private void ServerOrUserStop() => cts.Cancel();
    }
}