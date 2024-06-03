using Services.Models._BaseModels;
using Services.Models.Chat;
using Services.Models.ChatMember;
using Services.Models.ChatMessage;
using Services.Models.SignalR;

namespace Services.Interfaces
{
    public interface ISignalRService : IServiceRegistrator
    {
        Task SendNewMessageAsync(
            SignalREventNotificationInfo eventInfo, 
            GetChatMessageModel message);
        Task SendNewChatAsync(
            SignalREventNotificationInfo eventInfo, 
            GetChatModel chat);
        Task SendNewMessageViewedInfoAsync(
            SignalREventNotificationInfo eventInfo, 
            Guid chatId, 
            GetChatMessageViewedInfoModel messageViewedInfo);
        Task SendChatNameHasBeenChangedAsync(
            SignalREventNotificationInfo eventInfo,
            Guid chatId,
            string newChatName);
        Task SendChatAvatarHasBeenChangedAsync(
            SignalREventNotificationInfo eventInfo,
            Guid chatId,
            AttachFileModel newChatAvatar);
        Task SendChatMemberHasBeenDeletedAsync(
            SignalREventNotificationInfo eventInfo,
            Guid chatId,
            Guid chatMemberId);
        Task SendChatMemberHasBeenAddedAsync(
            SignalREventNotificationInfo eventInfo,
            Guid chatId,
            GetChatMemberModel newChatMember);
    }
}
