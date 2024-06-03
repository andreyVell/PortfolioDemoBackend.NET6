using WebApi.DTOs.ChatMember;
using WebApi.DTOs.ChatMessage;

namespace WebApi.DTOs.Chat
{
    public class CreateChatRequest
    {
        public string? Name { get; set; }
        public bool? IsGroupChat { get; set; }
        public List<CreateChatMemberForNewChatRequest>? ChatMembers { get; set; }
        public List<CreateChatFirstMessageRequest>? Messages { get; set; }
    }
}
