namespace RealTimeChat.Models
{
    public class GroupMember
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int GroupId { get; set; }
        public Group GroupChat { get; set; }
        public int Id { get; set; }
    }
}
