using DataCore.Entities;
using Microsoft.AspNetCore.SignalR;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Chat;
using Services.Models.ChatMember;
using Services.Models.ChatMessage;
using Services.Models.SignalR;
using Services.SignalRHubs;

namespace Services.Implementations
{
    public class SignalRService : ISignalRService
    {
        protected readonly IHubContext<ChatsHub> _chatsHubContext;
        protected readonly IGlobalSettings _globalSettings;

        public SignalRService(
            IHubContext<ChatsHub> chatsHubContext, 
            IGlobalSettings globalSettings)
        {
            _chatsHubContext = chatsHubContext;
            _globalSettings = globalSettings;
        }
        public async Task SendNewMessageAsync(SignalREventNotificationInfo eventInfo, GetChatMessageModel message)
        {
            eventInfo.MethodName = _globalSettings.SignalRMethodNameNewMessageCreated;
            await TaskSendObjectAsync(eventInfo, message);
        }

        public async Task SendNewChatAsync(SignalREventNotificationInfo eventInfo, GetChatModel chat)
        {
            eventInfo.MethodName = _globalSettings.SignalRMethodNameNewChatCreated;
            await TaskSendObjectAsync(eventInfo, chat);
        }

        public async Task SendNewMessageViewedInfoAsync(SignalREventNotificationInfo eventInfo, Guid chatId, GetChatMessageViewedInfoModel messageViewedInfo)
        {
            eventInfo.MethodName = _globalSettings.SignalRMethodNameNewMessageViewedInfoCreated;
            await TaskSendObjectAsync(eventInfo, messageViewedInfo, chatId);
        }

        public async Task SendChatNameHasBeenChangedAsync(SignalREventNotificationInfo eventInfo, Guid chatId, string newChatName)
        {
            eventInfo.MethodName = _globalSettings.SignalRMethodNameChatNameUpdated;
            await TaskSendObjectAsync(eventInfo, chatId, newChatName);
        }

        public async Task SendChatAvatarHasBeenChangedAsync(SignalREventNotificationInfo eventInfo, Guid chatId, AttachFileModel newChatAvatar)
        {
            eventInfo.MethodName = _globalSettings.SignalRMethodNameChatAvatarUpdated;
            await TaskSendObjectAsync(eventInfo, chatId, newChatAvatar);
        }

        public async Task SendChatMemberHasBeenDeletedAsync(SignalREventNotificationInfo eventInfo, Guid chatId, Guid chatMemberId)
        {
            eventInfo.MethodName = _globalSettings.SignalRMethodNameChatMemberDeleted;
            await TaskSendObjectAsync(eventInfo, chatId, chatMemberId);
        }

        public async Task SendChatMemberHasBeenAddedAsync(SignalREventNotificationInfo eventInfo, Guid chatId, GetChatMemberModel newChatMember)
        {
            eventInfo.MethodName = _globalSettings.SignalRMethodNameChatMemberAdded;
            await TaskSendObjectAsync(eventInfo, chatId, newChatMember);
        }

        private async Task TaskSendObjectAsync(SignalREventNotificationInfo eventInfo, object objectToSend, object? objectToSend2 = null)
        {
            await _chatsHubContext.Clients            
                .Groups(eventInfo.GroupNames)
                .SendAsync(eventInfo.MethodName, objectToSend, objectToSend2);
            await _chatsHubContext.Clients                
                .GroupExcept(eventInfo.EventOriginGroupName, eventInfo.EventOriginConnectionId)
                .SendAsync(eventInfo.MethodName, objectToSend, objectToSend2);
        }                
    }
}
