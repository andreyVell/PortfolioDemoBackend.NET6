using Services.Models._BaseModels;
using Services.Models.Authentication;
using Services.Models.Chat;
using Services.Models.ChatMessage;
using Services.Models.Project;
using Services.Models.ProjectStage;

namespace Services.Interfaces
{
    public interface IClientsViewService : IServiceRegistrator
    {
        Task<string> LoginAsync(LoginClientModel user);
        Task<PageModel<GetProjectModel>> GetProjectPageForCurrentUserAsync(int startIndex = 0, int itemsPerPage = 50, string filterString = "");
        Task<GetProjectModel> GetProjectDetailsAsync(Guid entityId);
        Task<List<GetProjectStageModel>> GetAllStagesForProjectAsync(Guid projectId, string? filterString = "");
        Task<GetProjectStageModel> GetProjectStageDetailsAsync(Guid projectStageId);
        Task<string> GetProjectNameForStageAsync(Guid projectStageId);
        Task<AttachFileModel> GetStageReportAttachedFileContentAsync(Guid stageReportAttachedFileId, bool isImageMedium = false);


        Task<AttachFileModel?> GetChatSmallAvatarAsync(Guid chatId);
        Task<PageModel<GetChatModel>> GetChatsPageAsync(int startIndex = 0, int itemsPerPage = 50, string filterString = "");
        Task<GetChatModel> GetChatAsync(Guid chatId);
        Task<SuccessfullCreateModel> CreateMessageAsync(CreateChatMessageModel newChatMessageModel, string eventOriginConnectionId);
        Task<PageModel<GetChatMessageModel>> GetMessagesForChatAsync(Guid chatId, int startIndex = 0, int itemsPerPage = 50);
        Task<GetChatMessageViewedInfoModel?> ViewMessageAsync(Guid messageId, Guid viewedByChatMemberId, string eventOriginConnectionId);
        Task<AttachFileModel?> GetChatMemberSmallAvatarAsync(Guid chatMemberId);
        Task<AttachFileModel?> GetChatMessageAttachedFileContentAsync(Guid chatMessageAttachedFileId, bool isImageMedium = false);
    }
}
