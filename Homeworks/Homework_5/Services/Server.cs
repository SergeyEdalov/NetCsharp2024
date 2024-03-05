using Homework_5.DTO;
using Homework_5.Models;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace Homework_5.Services
{
    //Реализуйте тип сообщений List, при котором клиент будет получать все
    //непрочитанные сообщения с сервера.
    //Дописать Метод Для команды Message
    public class Server
    {
        TcpListener? _tcpListener;

        public Server()
        {
             _tcpListener = new TcpListener(IPAddress.Any, 12345);
        }

        public void Run()
        {
            _tcpListener?.Start();

            Console.WriteLine("Запущено");

            while (true)
            {
                TcpClient? client = _tcpListener.AcceptTcpClient();
                Console.WriteLine("Успешно подключен");
                Task.Run(() => ProcessClient(client));
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
                    Message(message);
                    //Получать сообщение от клиента и отправлять другому клиенту
                    // при условии что IsReceived == false??
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

            if (message != null) 
                message.IsReceived = true;

            context.SaveChanges();
        }

        private void Message(TcpMessage tcpMessage)
        {
            using var context = new ChatContext();
            
            Message message = new Message() 
            { 
                Id = tcpMessage.Id , 
                Content = tcpMessage.Text,
                IsReceived = false,
                Author = new User() { UserName = tcpMessage.SenderName },
                Consumer = new User() { UserName = tcpMessage.ConsumerName },
            };

            context.Messages.Add(message);

            int count = 0;
            foreach (var user in context.Users)
            {
                if (user.UserName == message.Author.UserName)
                    user.SendedMessages.Add(message);
                    count++;

                if (user.UserName == message.Consumer.UserName)
                    user.RecievedMessages.Add(message);
                    count++;
            }
            if (count == 2) message.IsReceived = true;
            else
            {
                Queue <Message> queueMessage = new Queue<Message>();
                queueMessage.Enqueue(message);
            }

            context.SaveChanges();
        }
    }
}