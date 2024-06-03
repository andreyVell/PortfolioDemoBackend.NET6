using DataCore.Enums;

namespace WebApi.DTOs.ChatMessage
{
    public class CreateChatFirstMessageRequest
    {        
        public string? Text { get; set; }
        public List<CreateChatMessageAttachedFileRequest>? AttachedFiles { get; set; }
    }
}
