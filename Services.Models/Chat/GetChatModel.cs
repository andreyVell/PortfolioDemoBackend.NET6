using Services.Models._BaseModels;
using Services.Models.ChatMember;
using Services.Models.ChatMessage;

namespace Services.Models.Chat
{
    public class GetChatModel : ModelBase
    {
        public string? Name { get; set; }
        public string? PathToAvatarImage { get; set; }
        public bool? IsGroupChat { get; set; }
        public int TotalMessagesCount { get; set; }

        public GetChatMessageModel? LastMessage {  get; set; }
        public virtual List<GetChatMemberModel>? ChatMembers { get; set; }
        public virtual List<GetChatMessageModel>? Messages { get; set; }
    }
}
