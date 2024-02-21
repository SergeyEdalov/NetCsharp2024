using System.Net.Sockets;
using System.Text;

namespace Client_Homework_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpClient tcpClient = new TcpClient();

            Thread.Sleep(1000);

            tcpClient.Connect("127.0.0.1", 12345);

            Console.WriteLine("Подключен к чату");

            var stream = new StreamWriter(tcpClient.GetStream());

            string inputMessage = "";

            while (true)
            {
                inputMessage = Console.ReadLine();
                stream.WriteLine(inputMessage);
                Console.WriteLine("Сообщение отправлено");
            }
        }
    }
}
