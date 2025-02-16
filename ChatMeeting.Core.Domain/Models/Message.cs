namespace ChatMeeting.Core.Domain.Models
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string MessageText { get; set; }
        public DateTime CreatedAt { get; set; }
        public Chat Chat { get; set; }
        public User Sender { get; set; }
    }
}