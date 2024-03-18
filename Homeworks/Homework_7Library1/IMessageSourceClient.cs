using NetMQ;

namespace Homework_7Library1
{
    public interface IMessageSourceClient
    {
        public void Start() { }

        private void ReceiveMessage(object sender, NetMQSocketEventArgs e) { }
    }
}
