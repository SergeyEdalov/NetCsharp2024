using NetMQ;
using NetMQ.Sockets;

namespace Client
{
    internal class Client
    {
        private RequestSocket _socket;

        public Client()
        {
            _socket = new RequestSocket();
        }

        public void Connect(string name)
        {
            _socket.Connect(name);
        }

        public void SendMessage(string message)
        {
            _socket.SendFrame(message);

            var responce = _socket.ReceiveFrameString();

            Console.WriteLine($"Ответ от сервера - {responce}");
        }

        public void Disconnect()
        {
            _socket.Close();
            _socket.Dispose();
        }
    }
}
