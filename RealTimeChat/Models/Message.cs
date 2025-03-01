namespace RealTimeChat.Models
{
    public class Message
    {
        public int Id { get; set; }  

        public int SenderId { get; set; }  
        public User Sender { get; set; }
        public string SenderName { get; set; }
        public int GroupId { get; set; }
        public Group GroupChat { get; set; }

        public string Content { get; set; }  
        public DateTime SentAt { get; set; }

        
    }
}
