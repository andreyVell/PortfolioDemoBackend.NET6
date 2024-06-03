using DataCore.Entities;
using Services.Models._BaseModels;
using Services.Models.ChatMessage;

namespace Services.Interfaces
{
    public interface IChatMessageService : ICrudService<ChatMessage, GetChatMessageModel, CreateChatMessageModel, UpdateChatMessageModel>
    {
        Task<SuccessfullCreateModel> CreateAsync(CreateChatMessageModel newChatMessageModel, string eventOriginConnectionId);
        Task<PageModel<GetChatMessageModel>> GetMessagesForChatAsync(Guid chatId, int startIndex = 0, int itemsPerPage = 50);
        Task<GetChatMessageViewedInfoModel?> ViewMessageAsync(Guid messageId, Guid viewedByChatMemberId, string eventOriginConnectionId);
        Task<AttachFileModel> GetChatMessageAttachedFileContentAsync(Guid chatMessageAttachedFileId, bool isImageMedium = false);
    }
}
