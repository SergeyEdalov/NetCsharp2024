using System.Net;
using System.Net.Sockets;

namespace Server_Homework_1
{
    internal class Program
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

                    Task.Run(() => ProcessClient(tcpClient));

                }
            }
            catch (Exception ex) 
            {
                await Console.Out.WriteLineAsync(ex.Message.ToString());
            }
        }

        public async Task ProcessClient(TcpClient tcpClient)
        {
            List<StreamReader> readers = new List<StreamReader>();
            //List<StreamWriter> writers = new List<StreamWriter>();
            
            var reader = new StreamReader(tcpClient.GetStream());
            //var writer = new StreamWriter(tcpClient.GetStream());
            
            readers.Add(reader);
            //writers.Add(writer);

            var message = await reader.ReadLineAsync();
            
            Console.WriteLine($"{message}");
            Console.WriteLine("*****");
            
            //foreach (StreamWriter w in writers)
            //{
            //    w.WriteLine(message);
            //}


        }
    }
}
