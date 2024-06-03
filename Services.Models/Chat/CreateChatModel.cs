using Services.Models.ChatMember;
using Services.Models.ChatMessage;

namespace Services.Models.Chat
{
    public class CreateChatModel
    {
        public string? Name { get; set; }
        public bool? IsGroupChat { get; set; }
        public List<CreateChatMemberForNewChatModel>? ChatMembers { get; set; }
        public List<CreateChatFirstMessageModel>? Messages { get; set; }
    }
}
