using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeChatClient.MVVM.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int GroupId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}

