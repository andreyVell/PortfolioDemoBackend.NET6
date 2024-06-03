using Services.Models._BaseModels;

namespace WebApi.DTOs.ChatMessage
{
    public class CreateChatMessageAttachedFileRequest
    {
        public AttachFileModel FileContent { get; set; } = null!;
    }
}
