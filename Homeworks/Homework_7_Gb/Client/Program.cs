namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();

            var task = Task.Run(() =>
            {
                client.Connect("tcp://localhost:5050");
                Console.WriteLine("Введите сообщение");

                var message = Console.ReadLine();

                client.SendMessage(message);
                client.Disconnect();
            });

            task.Wait();
        }
    }
}
