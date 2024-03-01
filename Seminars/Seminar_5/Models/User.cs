using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seminar_5.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<Message> SendedMessages { get; set; }
        public virtual ICollection<Message> RecievedMessages { get; set; }
    }
}
