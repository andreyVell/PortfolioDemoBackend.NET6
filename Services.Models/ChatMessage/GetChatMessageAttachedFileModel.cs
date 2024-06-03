using Services.Models._BaseModels;

namespace Services.Models.ChatMessage
{
    public class GetChatMessageAttachedFileModel : ModelBase
    {
        public Guid ChatId { get; set; }
        public Guid MessageId { get; set; }
        public string? FileName { get; set; }
    }
}
