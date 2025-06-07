namespace FraokhChatBlazor.Client.Model
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string ConnectionId { get; set; }
    }
}
