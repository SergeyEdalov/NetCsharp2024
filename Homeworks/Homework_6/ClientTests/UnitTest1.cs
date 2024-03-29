using Cl_Homework_6;
using Ser_Homework_6;
using Moq;

namespace ClientTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException), "Method should throw Exception")]
        public void TestClient()
        {
            var server = new Mock<ChatServer>();

            server.Setup(x => x.Run());

            Task task = Task.Run(Client.Main);
            
            Assert.IsTrue(task.IsCompletedSuccessfully);
        }
    }
}