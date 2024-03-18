using NetMQ;
using NetMQ.Sockets;

namespace Homework_7Library1
{
    public class Source : IMessageSource
    {
        public void SendMessage (string message)
        {
            using(var publisher = new PublisherSocket())
            {
                publisher.Bind("");
                publisher.SendFrame(message);
            }
        }
    }
}
