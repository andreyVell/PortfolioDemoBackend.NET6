using DataCore.Entities;
using Services.Models._BaseModels;
using Services.Models.Chat;
using Services.Models.ChatMember;

namespace Services.Interfaces
{
    public interface IChatService : ICrudService<Chat, GetChatModel, CreateChatModel, UpdateChatModel>
    {
        Task<AttachFileModel?> GetChatSmallAvatarAsync(Guid chatId);
        Task<AttachFileModel?> GetChatBigAvatarAsync(Guid chatId);
        Task<GetChatModel> CreateAsync(CreateChatModel newModel, string eventOriginConnectionId);
        Task<SuccessfullUpdateModel> UpdateAsync(UpdateChatModel updateModel, string eventOriginConnectionId);
        Task<GetChatModel?> GetPersonalChatForInterlocutorAsync(CreateChatMemberForNewChatModel chatMember);
        Task RemoveChatMemberFromChatAsync(Guid chatId, Guid chatMemberId);
        Task<List<GetChatMemberModel>> AddChatMembersToChatAsync(Guid chatId, List<CreateChatMemberForNewChatModel> chatMembers);

    }
}
