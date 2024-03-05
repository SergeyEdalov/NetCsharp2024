using Homework_5.Services;
using System.Net;

namespace Homework_5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
        }
    }
}
