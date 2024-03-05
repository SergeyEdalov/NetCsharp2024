
namespace Homework_5.Models
{
    public class Message
    {
        public int? Id { get; set; }
        public int? AuthorId { get; set; }
        public int? ConsumerId { get; set; }
        public string Content { get; set; }
        public bool IsReceived { get; set; }
        public virtual User? Author { get; set; }
        public virtual User? Consumer { get; set; }
    }
}
