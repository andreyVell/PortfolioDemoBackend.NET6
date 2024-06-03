using DataCore.Entities;
using Services.Models._BaseModels;
using Services.Models.ChatMember;

namespace Services.Interfaces
{
    public interface IChatMemberService : ICrudService<ChatMember, GetChatMemberModel, CreateChatMemberModel, UpdateChatMemberModel>
    {
        Task<AttachFileModel?> GetChatMemberSmallAvatarAsync(Guid chatMemberId);
        Task<PageModel<GetChatMemberModel>> GetPotentialChatMembersForChatAsync(int startIndex = 0, int itemsPerPage = 50, string? filterString = "", Guid? chatId = null);
    }
}
