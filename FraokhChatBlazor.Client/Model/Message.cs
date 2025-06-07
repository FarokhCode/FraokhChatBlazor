namespace FraokhChatBlazor.Client.Model
{
    public class Message
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
