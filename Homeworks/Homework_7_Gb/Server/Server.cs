using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class Server
    {
        private ResponseSocket _socket;

        public Server()
        {
            _socket = new ResponseSocket();
        }

        public void Start()
        {
            _socket.Bind("tcp://localhost:5050");

            while (true)
            {
                var message = _socket.ReceiveFrameString();

                Console.WriteLine(message);

                var response = "";

                _socket.SendFrame(response);
            }
        }

        public void Stop()
        {
            _socket.Close();
            _socket.Dispose();
        }
    }
}
