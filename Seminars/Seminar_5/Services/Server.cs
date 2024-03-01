using Seminar_5.DTO;
using Seminar_5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_5.Services
{
    public class Server
    {
        TcpListener? _tcpListener;

        public Server(IPEndPoint? endPoint)
        {
            if (endPoint != null)
                _tcpListener = new TcpListener(endPoint);
        }

        public void Run()
        {
            _tcpListener?.Start();

            Console.WriteLine("Запущено");

            while (true)
            {
                TcpClient? client = _tcpListener.AcceptTcpClient();
  
            }
        }

        async Task ProcessClient(TcpClient client)
        {
            var reader = new StreamReader(client.GetStream());

            string? json;

            json = await reader.ReadToEndAsync();

            var message = TcpMessage.JsonToMessage(json);

            switch (message?.Status)
            {
                case Command.Registered:
                    RegistrerClient(message.SenderName);
                    break;
                case Command.Confirmed:
                    Confirmed(message.Id);
                    break;
                case Command.Message:
                    //Получать сообщение от клиента и отправлять другому клиенту
                    // при условии что IsReceived == false
                    break;
            }
        }

        private void RegistrerClient(string name)
        {
            using var context = new ChatContext();
            context.Users.Add(new User { UserName = name });
            context.SaveChanges();
        }

        private void Confirmed(int? id)
        {
            using var context = new ChatContext();
            var message = context.Messages.FirstOrDefault(m => m.Id == id);

            message.IsReceived = true;
            context.SaveChanges();
        }
    }
}