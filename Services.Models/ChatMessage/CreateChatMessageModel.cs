using Services.Models._BaseModels;

namespace Services.Models.ChatMessage
{
    public class CreateChatMessageModel
    {
        public Guid OwnerId { get; set; }
        public Guid ChatId { get; set; }
        public Guid? RecieverId { get; set; }
        public string? Text { get; set; }
        public List<AttachFileModel>? AttachFiles { get; set; }
    }
}
