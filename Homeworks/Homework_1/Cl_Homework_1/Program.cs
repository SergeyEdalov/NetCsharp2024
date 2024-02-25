using System.Net.Sockets;

namespace Cl_Homework_1
{
    public class Client
    {
        static async void Main(string[] args)
        {
            using TcpClient tcpClient = new TcpClient();
            Console.Write("Введите свое имя: ");
            string? userName = Console.ReadLine();
            Console.WriteLine($"Добро пожаловать, {userName}");
            Thread.Sleep(1000);
            try
            {
                tcpClient.Connect("127.0.0.1", 12345);

                Console.WriteLine("Подключен к чату");

                var reader = new StreamReader(tcpClient.GetStream());
                var writer = new StreamWriter(tcpClient.GetStream());
                if (writer is null || reader is null) return;
                Task.Run(() => ReceiveMessage(reader));
                await SendMessage(writer);
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            async Task SendMessage(StreamWriter writer)
            {
                // сначала отправляем имя
                await writer.WriteLineAsync(userName);
                await writer.FlushAsync();
                Console.WriteLine("Для отправки сообщений введите сообщение и нажмите Enter");

                while (true)
                {
                    string? message = Console.ReadLine();
                    await writer.WriteLineAsync(message);
                    await writer.FlushAsync();
                }
            }

            async Task ReceiveMessage(StreamReader reader)
            {
                while (true)
                {
                    string? message = await reader.ReadLineAsync();
                    Console.WriteLine(message);
                }
            }
        }
    }
}
