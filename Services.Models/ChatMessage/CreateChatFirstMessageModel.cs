using Services.Models._BaseModels;

namespace Services.Models.ChatMessage
{
    public class CreateChatFirstMessageModel
    {
        public string? Text { get; set; }
        public List<AttachFileModel>? AttachFiles { get; set; }
    }
}
