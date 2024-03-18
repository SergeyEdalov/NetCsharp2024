namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new Server();

            var task = Task.Run(server.Start);

            task.Wait();
        }
    }
}
