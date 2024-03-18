using NetMQ;
using NetMQ.Sockets;

namespace Homework_7Library1
{
    public class SourceClient : IMessageSourceClient
    {
        private SubscriberSocket? subscriber;


        public void Start()
        {
            subscriber = new SubscriberSocket();

            subscriber.Connect("");

            subscriber.SubscribeToAnyTopic();

            subscriber.ReceiveReady += ReceiveMessage;

            subscriber.Poll();
        }


        private void ReceiveMessage (object? sender, NetMQSocketEventArgs e)
        {
            var message = e.Socket.ReceiveFrameString();
        }
    }
}
