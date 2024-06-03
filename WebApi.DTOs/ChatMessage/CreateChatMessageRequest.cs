namespace WebApi.DTOs.ChatMessage
{
    public class CreateChatMessageRequest
    {
        public Guid OwnerId { get; set; }
        public Guid ChatId { get; set; }
        public Guid? RecieverId { get; set; }
        public string? Text { get; set; }
        public List<CreateChatMessageAttachedFileRequest>? AttachedFiles { get; set; }
    }
}
