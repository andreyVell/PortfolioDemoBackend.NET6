using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Chat;
using Services.Models.ChatMember;
using Services.Models.ChatMessage;
using Services.Models.SignalR;

namespace Services.Implementations
{
    public class ChatService : CrudService<Chat, GetChatModel, CreateChatModel, UpdateChatModel>, IChatService
    {
        protected readonly IFileProviderService _fileProviderService;
        protected readonly IGlobalSettings _globalSettings;
        protected readonly ISignalRService _signalRService;

        //TODO при удалении чата у всех chatMessageAttachedFiles из этого чата становится chatId = null, их надо бы удалять

        public ChatService(
            IGlobalSettings globalSettings,
            IAvetonDbContext avetonDbContext,
            IFileProviderService fileProviderService,
            IMapper mapper, 
            ICurrentUserDataService currentUserService,
            ISignalRService signalRService) : base(avetonDbContext, mapper, currentUserService)
        {
            _globalSettings = globalSettings;
            _fileProviderService = fileProviderService;
            _signalRService = signalRService;
        }

        public async Task<GetChatModel> CreateAsync(CreateChatModel newModel, string eventOriginConnectionId)
        {            
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var newChat = _mapper.Map<Chat>(newModel);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            newChat.CreatedByUser = currentUserLogin;
            newChat.UpdatedByUser = currentUserLogin;
            newChat.EntityOwnerId = _currentUserOwnerId;
                          
            if (newChat.ChatMembers != null)
            {
                foreach (var chatMember in newChat.ChatMembers)
                {
                    chatMember.CreatedByUser = currentUserLogin;
                    chatMember.UpdatedByUser = currentUserLogin;
                    chatMember.EntityOwnerId = _currentUserOwnerId;
                    chatMember.ChatId = newChat.Id;
                }                
            }

            var chatMemberSelf = new ChatMember();
            chatMemberSelf.CreatedByUser = currentUserLogin;
            chatMemberSelf.UpdatedByUser = currentUserLogin;
            chatMemberSelf.EntityOwnerId = _currentUserOwnerId;
            chatMemberSelf.ChatId = newChat.Id;
            chatMemberSelf.Type = DataCore.Enums.ChatMemberType.Employee;
            chatMemberSelf.EmployeeId = await _currentUserService.GetEmployeeIdForCurrentUserAsync();
            
            if (newChat.ChatMembers == null)
            {
                newChat.ChatMembers = new List<ChatMember>() { chatMemberSelf };
            }
            else
            {
                newChat.ChatMembers.Add(chatMemberSelf);
            }

            if (newChat.IsGroupChat == true)
            {
                var systemMessage = new ChatMessage();
                systemMessage.IsSystem = true;
                systemMessage.Text = "Чат создан";
                if (newChat.Messages == null)
                {
                    newChat.Messages = new List<ChatMessage>();
                }
                newChat.Messages.Add(systemMessage);
            }

            if (newChat.Messages != null)
            {
                foreach(var firstMessage in newChat.Messages)
                {
                    firstMessage.CreatedByUser = currentUserLogin;
                    firstMessage.UpdatedByUser = currentUserLogin;
                    firstMessage.EntityOwnerId = _currentUserOwnerId;
                    if (firstMessage.IsSystem == true)
                    {
                        firstMessage.OwnerId = null;
                    }
                    else
                    {
                        firstMessage.OwnerId = chatMemberSelf.Id;
                    }
                    
                    firstMessage.ChatId = newChat.Id;                    
                }                
            }

            var result = await _avetonDbContext.InsertAsync(newChat);
            var lastMessage = newChat?.Messages?.LastOrDefault();
            if (lastMessage != null)
            {
                var attachedFiles = newModel?.Messages?.SelectMany(e => e.AttachFiles!);
                if (attachedFiles != null)
                {
                    foreach (var file in attachedFiles)
                    {
                        var chatMessageAttachFile = new ChatMessageAttachedFile();
                        chatMessageAttachFile.CreatedByUser = currentUserLogin;
                        chatMessageAttachFile.UpdatedByUser = currentUserLogin;
                        chatMessageAttachFile.EntityOwnerId = _currentUserOwnerId;
                        chatMessageAttachFile.ChatId = result.Id;
                        chatMessageAttachFile.MessageId = lastMessage.Id;
                        chatMessageAttachFile.FileName = file.FileName;
                        chatMessageAttachFile.FilePath = await _fileProviderService.SaveFileAsync(file);
                        if (ImageFormatter.IsBase64StringIsImage(file.FileContent))
                        {
                            chatMessageAttachFile.ImageMediumSizeFilePath = await _fileProviderService.CompressAndSaveImageAsync(file, 400, 600);
                        }
                        await _avetonDbContext.InsertAsync(chatMessageAttachFile);
                    }
                }                
            }
            result = await _avetonDbContext.Chats
                .Include(e => e.Messages)
                .Include(e => e.ChatMembers).ThenInclude(e => e.Employee)
                .Include(e => e.ChatMembers).ThenInclude(e => e.PersonClient)
                .Include(e => e.ChatMembers).ThenInclude(e => e.OrganizationClient)
                .FirstOrDefaultAsync(e => e.Id == result.Id);
            var recepientIds = result?.ChatMembers
                    .Where(e => e.Id != chatMemberSelf.Id)
                    .Select(e => e.Employee?.CredentialsId ?? e.OrganizationClientId ?? e.PersonClientId)
                    .Where(e => e != null)
                    .Select(e => e.ToString())
                    .ToList();
            var recepientIdSelf = result?.ChatMembers
                    .FirstOrDefault(e => e.Id == chatMemberSelf.Id)
                    ?.Employee?.CredentialsId?.ToString() ?? string.Empty;

            var getChatModel = _mapper.Map<GetChatModel>(result);
            getChatModel.Messages = _mapper.Map<List<GetChatMessageModel>>(result!.Messages.OrderByDescending(e => e.CreatedOn));

            if (recepientIds != null)
            {
                await _signalRService.SendNewChatAsync(
                    new SignalREventNotificationInfo()
                    {
                        EventOriginConnectionId = eventOriginConnectionId,
                        GroupNames = recepientIds!,
                        EventOriginGroupName = recepientIdSelf
                    },
                    getChatModel
                );
            }            
            return getChatModel;
        }

        public async Task<SuccessfullUpdateModel> UpdateAsync(UpdateChatModel updateModel, string eventOriginConnectionId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbModel = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<Chat>(x => x.Id == updateModel.Id && x.EntityOwnerId == _currentUserOwnerId);
            if (dbModel == null)
            {
                throw new EntityNotFoundException(nameof(Chat));
            }
            CheckLostUpdate(dbModel, updateModel);
            var curentLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            var recepientIdsForSystemMessage = await GetRecepientIdsForSystemMessageAsync(dbModel);
            var currentUserEmployeeId = await _currentUserService.GetEmployeeIdForCurrentUserAsync();
            var recepientIds = await GetRecepientIdsExcludeSelfAsync(dbModel, currentUserEmployeeId);
            var recepientIdSelf = await GetRecepientIdSelfAsync(dbModel, currentUserEmployeeId);

            if (updateModel.Name != dbModel.Name)
            {
                var systemMessage =
                    GenerateSystemChatMessage(dbModel.Id, dbModel.EntityOwnerId, curentLogin, $"Назавние чата изменено: {dbModel.Name} -> {updateModel.Name}");                
                dbModel.Name = updateModel.Name;
                await _avetonDbContext.InsertAsync(systemMessage);
                await SendSystemMessageAsync(recepientIdsForSystemMessage, systemMessage);
                if (recepientIds != null)
                {
                    await _signalRService.SendChatNameHasBeenChangedAsync(
                        new SignalREventNotificationInfo()
                        {
                            EventOriginConnectionId = eventOriginConnectionId,
                            GroupNames = recepientIds!,
                            EventOriginGroupName = recepientIdSelf
                        }, dbModel.Id, dbModel.Name ?? string.Empty);
                }

            }
            if (updateModel.NewAvatar != null)
            {
                if (!ImageFormatter.IsBase64StringIsImage(updateModel.NewAvatar.FileContent))
                {
                    throw new FileIsNotImageException();
                }
                if (!string.IsNullOrWhiteSpace(dbModel.PathToAvatarBigImage))
                {
                    await _fileProviderService.DeleteAsync(dbModel.PathToAvatarBigImage);
                }
                if (!string.IsNullOrWhiteSpace(dbModel.PathToAvatarSmallImage))
                {
                    await _fileProviderService.DeleteAsync(dbModel.PathToAvatarSmallImage);
                }
                dbModel.PathToAvatarBigImage = await _fileProviderService.SaveFileAsync(updateModel.NewAvatar);
                dbModel.PathToAvatarSmallImage = await _fileProviderService.CompressAndSaveImageAsync(updateModel.NewAvatar, 60, 60);
                var systemMessage = 
                    GenerateSystemChatMessage(dbModel.Id, dbModel.EntityOwnerId, curentLogin, "Фотография чата изменена");                
                await _avetonDbContext.InsertAsync(systemMessage);
                await SendSystemMessageAsync(recepientIdsForSystemMessage, systemMessage);
                if (recepientIds != null)
                {
                    await _signalRService.SendChatAvatarHasBeenChangedAsync(
                        new SignalREventNotificationInfo()
                        {
                            EventOriginConnectionId = eventOriginConnectionId,
                            GroupNames = recepientIds!,
                            EventOriginGroupName = recepientIdSelf
                        }, dbModel.Id, updateModel.NewAvatar);
                }
            }
            var result = await _avetonDbContext.UpdateAsync(dbModel);
            return new SuccessfullUpdateModel(result);
        }

        public async Task<AttachFileModel?> GetChatSmallAvatarAsync(Guid chatId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbChat = await _avetonDbContext.GetFirstOrDefaultAsync<Chat>(x => x.Id == chatId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbChat == null)
            {
                throw new EntityNotFoundException(nameof(Chat));
            }            
            return await _fileProviderService.GetFileDataUrlAsync(dbChat.PathToAvatarSmallImage);               
        }

        public async Task<AttachFileModel?> GetChatBigAvatarAsync(Guid chatId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbChat = await _avetonDbContext.GetFirstOrDefaultAsync<Chat>(x => x.Id == chatId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbChat == null)
            {
                throw new EntityNotFoundException(nameof(Chat));
            }
            return await _fileProviderService.GetFileDataUrlAsync(dbChat.PathToAvatarBigImage);
        }

        public async override Task<PageModel<GetChatModel>> GetPageAsync(int startIndex = 0, int itemsPerPage = 50, string filterString = "")
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var currentUserEmployeeId = await _currentUserService.GetEmployeeIdForCurrentUserAsync();
            var filterQuery = GetFilterQuery(filterString)
                .Where(x => x.EntityOwnerId == _currentUserOwnerId
                    && (x.LastMessageProjectable != null || x.IsGroupChat == true)
                    && x.ChatMembers.Any(e => e.Employee!.CredentialsId == _currentUserService.GetCurrentUserId()));

            var totalItems = await filterQuery.CountAsync();

            var chatsList =
                await OrderByConditionQuery(filterQuery)
                .Skip(startIndex)
                .Take(itemsPerPage)
                .ToListAsync();

            var resultItems = new List<GetChatModel>(chatsList.Count);

            foreach (var chat in chatsList)
            {
                var getChatModel = _mapper.Map<GetChatModel>(chat);
                getChatModel.TotalMessagesCount = chat.Messages.Count();
                getChatModel.Messages = _mapper.Map<List<GetChatMessageModel>>(
                    chat.Messages
                    .OrderByDescending(e => e.CreatedOn)
                    .Take(_globalSettings.FirstLoadedMessagesPerChat));
                resultItems.Add(getChatModel);
            }
            var result = new PageModel<GetChatModel>
            {
                Items = resultItems,
                TotalItems = totalItems,
                StartIndex = startIndex,
                ItemsPerPage = itemsPerPage,
            };

            return result;
        }            

        public async Task<GetChatModel?> GetPersonalChatForInterlocutorAsync(CreateChatMemberForNewChatModel interlocutor)
        {            
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            Chat? chat = null;
            var currentUserEmployeeId = await _currentUserService.GetEmployeeIdForCurrentUserAsync();
            switch (interlocutor.Type)
            {
                case DataCore.Enums.ChatMemberType.Employee:
                    chat = await _avetonDbContext.Chats
                        .FirstOrDefaultAsync(e => e.IsGroupChat == false
                        && e.ChatMembers.Any(x => x.EmployeeId == interlocutor.EmployeeId)
                        && e.ChatMembers.Any(x => x.EmployeeId == currentUserEmployeeId));
                    break;
                case DataCore.Enums.ChatMemberType.PersonClient:
                    chat = await _avetonDbContext.Chats
                        .FirstOrDefaultAsync(e => e.IsGroupChat == false 
                        && e.ChatMembers.Any(x => x.PersonClientId == interlocutor.PersonClientId)
                        && e.ChatMembers.Any(x => x.EmployeeId == currentUserEmployeeId));
                    break;
                case DataCore.Enums.ChatMemberType.OrganizationClient:
                     chat = await _avetonDbContext.Chats
                        .FirstOrDefaultAsync(e => e.IsGroupChat == false 
                        && e.ChatMembers.Any(x => x.OrganizationClientId == interlocutor.OrganizationClientId)
                        && e.ChatMembers.Any(x => x.EmployeeId == currentUserEmployeeId));
                    break;
            }
            if (chat == null) return null;
            var mappedChat = _mapper.Map<GetChatModel>(chat);
            mappedChat.Messages = _mapper.Map<List<GetChatMessageModel>>(chat!.Messages.OrderByDescending(e => e.CreatedOn));
            return mappedChat;
        }

        public async Task RemoveChatMemberFromChatAsync(Guid chatId, Guid chatMemberId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbChat = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<Chat>(x => x.Id == chatId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbChat == null)
            {
                throw new EntityNotFoundException(nameof(Chat));
            }
            var dbChatMemberToDelete = await _avetonDbContext.ChatMembers.FirstOrDefaultAsync(x => x.Id == chatMemberId && x.ChatId == chatId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbChatMemberToDelete != null)
            {                
                string systemMessageText = "Пользователь ";
                switch (dbChatMemberToDelete.Type)
                {
                    case DataCore.Enums.ChatMemberType.Employee:
                        systemMessageText += dbChatMemberToDelete.Employee?.FirstNameAndLastName ?? string.Empty;
                        break;
                    case DataCore.Enums.ChatMemberType.PersonClient:
                        systemMessageText += dbChatMemberToDelete.PersonClient?.FirstNameAndLastName ?? string.Empty;
                        break;
                    case DataCore.Enums.ChatMemberType.OrganizationClient:
                        systemMessageText += dbChatMemberToDelete.OrganizationClient?.Name ?? string.Empty;
                        break;
                    default:
                        break;
                }
                systemMessageText += " был удалён из чата";
                var curentLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
                var recepientIds = await GetRecepientIdsForSystemMessageAsync(dbChat);
                await _avetonDbContext.DeleteAsync(dbChatMemberToDelete);
                var systemMessage =
                    GenerateSystemChatMessage(dbChat.Id, dbChat.EntityOwnerId, curentLogin, systemMessageText);
                await _avetonDbContext.InsertAsync(systemMessage);
                await SendSystemMessageAsync(dbChat, systemMessage, new List<Guid>() { chatMemberId });
                await SendChatMemberWasDeletedAsync(dbChat, chatMemberId, recepientIds);
            }
        }

        public async Task<List<GetChatMemberModel>> AddChatMembersToChatAsync(Guid chatId, List<CreateChatMemberForNewChatModel> chatMembers)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbChat = await _avetonDbContext.GetFirstOrDefaultAsync<Chat>(x => x.Id == chatId && x.EntityOwnerId == _currentUserOwnerId) 
                ?? throw new EntityNotFoundException(nameof(Chat));
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            var newChatMembers = _mapper.Map<List<ChatMember>>(chatMembers);
            var addedChatMembersResult = new List<GetChatMemberModel>();
            foreach (var chatMember in newChatMembers)
            {
                if (dbChat.ChatMembers.Any(e => (e.EmployeeId != null && e.EmployeeId == chatMember.EmployeeId)
                    || (e.OrganizationClientId != null && e.OrganizationClientId == chatMember.OrganizationClientId)
                    || (e.PersonClientId != null && e.PersonClientId == chatMember.PersonClientId)))
                {
                    //Нельзя добавить пользователей которе уже были в чате
                    continue;
                }
                chatMember.CreatedByUser = currentUserLogin;
                chatMember.UpdatedByUser = currentUserLogin;
                chatMember.EntityOwnerId = _currentUserOwnerId;
                chatMember.ChatId = dbChat.Id;
                var recipientsForNewChatMemberEventMessage = await GetRecepientIdsForSystemMessageAsync(dbChat);
                await _avetonDbContext.InsertAsync(chatMember);
                var chatMemberDb = await _avetonDbContext.ChatMembers
                    .Where(e => e.Id == chatMember.Id)
                    .Include(e => e.Employee)
                    .Include(e => e.PersonClient)
                    .Include(e => e.OrganizationClient)
                    .FirstOrDefaultAsync();            
                string systemMessageText = "Пользователь ";
                switch (chatMemberDb!.Type)
                {
                    case DataCore.Enums.ChatMemberType.Employee:
                        systemMessageText += chatMemberDb.Employee?.FirstNameAndLastName ?? string.Empty;
                        break;
                    case DataCore.Enums.ChatMemberType.PersonClient:
                        systemMessageText += chatMemberDb.PersonClient?.FirstNameAndLastName ?? string.Empty;
                        break;
                    case DataCore.Enums.ChatMemberType.OrganizationClient:
                        systemMessageText += chatMemberDb.OrganizationClient?.Name ?? string.Empty;
                        break;
                    default:
                        break;
                }
                systemMessageText += " добавлен в чат";
                var curentLogin = (await _currentUserService.GetCurrentUserAsync()).Login;                
                var systemMessage =
                    GenerateSystemChatMessage(dbChat.Id, dbChat.EntityOwnerId, curentLogin, systemMessageText);
                await _avetonDbContext.InsertAsync(systemMessage);
                await SendSystemMessageAsync(dbChat, systemMessage);
                var addedChatMember = _mapper.Map<GetChatMemberModel>(chatMemberDb);
                await SendChatMemberWasAddedAsync(dbChat, addedChatMember, recipientsForNewChatMemberEventMessage);
                addedChatMembersResult.Add(addedChatMember);
            }
            return addedChatMembersResult;
        }

        public override async Task<GetChatModel> GetAsync(Guid entityId)
        { 
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var currentUserEmployeeId = await _currentUserService.GetEmployeeIdForCurrentUserAsync();
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<Chat>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId
                && x.ChatMembers.Any(e => e.EmployeeId == currentUserEmployeeId));
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(Chat));
            }
            var mappedChat = _mapper.Map<GetChatModel>(model);
            mappedChat.Messages = _mapper.Map<List<GetChatMessageModel>>(model!.Messages.OrderByDescending(e => e.CreatedOn));
            return mappedChat;
        }

        protected async override Task<bool> CanDeleteEntityAsync(Chat entityToDelete)
        {
            return true;
        }

        protected override IQueryable<Chat> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Chats
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                filterQuery = filterQuery.Where(e =>
                    e.IsGroupChat == true && e.Name!.ToLower().Contains(filterString)
                    || e.Messages.Any(x => x.Text!.ToLower().Contains(filterString))
                    || e.ChatMembers.Any(x => x.Employee!.FirstName!.ToLower().Contains(filterString)
                                                || x.Employee.LastName!.ToLower().Contains(filterString)
                                                || x.Employee!.SecondName!.ToLower().Contains(filterString)
                                            || x.PersonClient!.FirstName!.ToLower().Contains(filterString)
                                                || x.PersonClient!.LastName!.ToLower().Contains(filterString)
                                                || x.PersonClient!.SecondName!.ToLower().Contains(filterString)
                                            || (e.IsGroupChat == true && e.Name!.ToLower().Contains(filterString)))
                    );
            }
            return filterQuery;
        }

        protected override IQueryable<Chat> OrderByConditionQuery(IQueryable<Chat> query)
        {
            return query.OrderByDescending(e => e.LastMessageProjectable!.CreatedOn).AsQueryable();
        }

        private ChatMessage GenerateSystemChatMessage(Guid chatId, Guid entityOwnerId, string currentUserLogin, string text)
        {
            var systemMessage = new ChatMessage();
            systemMessage.ChatId = chatId;
            systemMessage.OwnerId = null;
            systemMessage.IsSystem = true;
            systemMessage.Text = text;
            systemMessage.EntityOwnerId = entityOwnerId;
            systemMessage.CreatedByUser = currentUserLogin;
            systemMessage.UpdatedByUser = currentUserLogin;
            return systemMessage;
        }     
        
        private async Task SendSystemMessageAsync(List<string> recepientIds, ChatMessage systemMessage)
        {
            if (recepientIds != null)
            {
                await _signalRService.SendNewMessageAsync(
                    new SignalREventNotificationInfo()
                    {
                        EventOriginConnectionId = string.Empty,
                        GroupNames = recepientIds!,
                        EventOriginGroupName = string.Empty
                    }, _mapper.Map<GetChatMessageModel>(systemMessage));
            }
        }

        private async Task SendSystemMessageAsync(Chat chat, ChatMessage systemMessage, List<Guid>? excludedMemberIds = null)
        {
            var recepientIds = await GetRecepientIdsForSystemMessageAsync(chat, excludedMemberIds);
            if (recepientIds != null)
            {
                await _signalRService.SendNewMessageAsync(
                    new SignalREventNotificationInfo()
                    {
                        EventOriginConnectionId = string.Empty,
                        GroupNames = recepientIds!,
                        EventOriginGroupName = string.Empty
                    }, _mapper.Map<GetChatMessageModel>(systemMessage));
            }
        }

        private async Task SendChatMemberWasDeletedAsync(Chat chat, Guid chatMemberId, List<string>? recepientIds)
        {
            if (recepientIds != null)
            {
                await _signalRService.SendChatMemberHasBeenDeletedAsync(
                    new SignalREventNotificationInfo()
                    {
                        EventOriginConnectionId = string.Empty,
                        GroupNames = recepientIds!,
                        EventOriginGroupName = string.Empty
                    }, chat.Id, chatMemberId);
            }
        }

        private async Task SendChatMemberWasAddedAsync(Chat chat, GetChatMemberModel addedChatMember, List<string>? recepientIds)
        {
            if (recepientIds != null)
            {
                await _signalRService.SendChatMemberHasBeenAddedAsync(
                    new SignalREventNotificationInfo()
                    {
                        EventOriginConnectionId = string.Empty,
                        GroupNames = recepientIds!,
                        EventOriginGroupName = string.Empty
                    }, chat.Id, addedChatMember);
            }
        }

        private async Task<List<string>> GetRecepientIdsForSystemMessageAsync(Chat chat, List<Guid>? excludedMemberIds = null)
        {
            if (excludedMemberIds == null)
            {
                excludedMemberIds = new List<Guid>();
            }
            return await _avetonDbContext.Chats.AsNoTracking()
                .Where(e => e.Id == chat.Id)
                .SelectMany(e => e.ChatMembers)
                .Where(e => !excludedMemberIds.Any(x => x == e.Id))
                .Select(e => e.Employee!.CredentialsId ?? e.OrganizationClientId ?? e.PersonClientId)
                .Where(e => e != null)
                .Select(e => e.ToString()!)
                .ToListAsync();
        }

        private async Task<List<string>> GetRecepientIdsExcludeSelfAsync(Chat chat, Guid? currentUserEmployeeId)
        {
            return await _avetonDbContext.Chats.AsNoTracking()
                .Where(e => e.Id == chat.Id)
                .SelectMany(e => e.ChatMembers)
                .Where(e => e.EmployeeId != currentUserEmployeeId)
                .Select(e => e.Employee!.CredentialsId ?? e.OrganizationClientId ?? e.PersonClientId)
                .Where(e => e != null)
                .Select(e => e.ToString()!)
                .ToListAsync();
        }

        private async Task<string> GetRecepientIdSelfAsync(Chat chat, Guid? currentUserEmployeeId)
        {
            return await _avetonDbContext.Chats.AsNoTracking()
                .Where(e => e.Id == chat.Id)
                .SelectMany(e => e.ChatMembers)
                .Where(e => e.EmployeeId == currentUserEmployeeId)
                .Select(e => e.Employee!.CredentialsId)
                .Where(e => e != null)
                .Select(e => e.ToString()!)
                .FirstOrDefaultAsync() ?? string.Empty;
        }
    }
}
