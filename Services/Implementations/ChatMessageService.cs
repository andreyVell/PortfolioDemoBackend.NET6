using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.ChatMessage;
using Services.Models.SignalR;

namespace Services.Implementations
{
    public class ChatMessageService : CrudService<ChatMessage, GetChatMessageModel, CreateChatMessageModel, UpdateChatMessageModel>, IChatMessageService
    {
        protected readonly ISignalRService _signalRService;
        protected readonly IFileProviderService _fileProviderService;


        //TODO при удалении сообщения у всех chatMessageAttachedFiles из этого сообщения становится messageId = null, их надо бы удалять
        public ChatMessageService(
            IAvetonDbContext avetonDbContext, 
            IMapper mapper, 
            ICurrentUserDataService currentUserService,
            ISignalRService signalRService,
            IFileProviderService fileProviderService) : base(avetonDbContext, mapper, currentUserService)
        {
            _signalRService = signalRService;
            _fileProviderService = fileProviderService;
        }

        public async Task<SuccessfullCreateModel> CreateAsync(CreateChatMessageModel newChatMessageModel, string eventOriginConnectionId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ChatMessage).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var newMessage = _mapper.Map<ChatMessage>(newChatMessageModel);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            newMessage.CreatedByUser = currentUserLogin;
            newMessage.UpdatedByUser = currentUserLogin;
            newMessage.EntityOwnerId = _currentUserOwnerId;
            var result = await _avetonDbContext.InsertAsync(newMessage);
            if (newChatMessageModel.AttachFiles != null)
            {
                foreach (var file in newChatMessageModel.AttachFiles)
                {
                    var chatMessageAttachFile = new ChatMessageAttachedFile();
                    chatMessageAttachFile.CreatedByUser = currentUserLogin;
                    chatMessageAttachFile.UpdatedByUser = currentUserLogin;
                    chatMessageAttachFile.EntityOwnerId = _currentUserOwnerId;
                    chatMessageAttachFile.ChatId = result.ChatId;
                    chatMessageAttachFile.MessageId = result.Id;
                    chatMessageAttachFile.FileName = file.FileName;
                    chatMessageAttachFile.FilePath = await _fileProviderService.SaveFileAsync(file);
                    if (ImageFormatter.IsBase64StringIsImage(file.FileContent))
                    {
                        chatMessageAttachFile.ImageMediumSizeFilePath = await _fileProviderService.CompressAndSaveImageAsync(file, 400, 600);
                    }
                    await _avetonDbContext.InsertAsync(chatMessageAttachFile);
                }
            }
            var recepientIds =
                (await _avetonDbContext.Chats.Where(e => e.Id == result.ChatId).Include(e => e.ChatMembers).FirstOrDefaultAsync())
                    ?.ChatMembers
                    .Where(e => e.Id != newMessage.OwnerId)
                    .Select(e => e.Employee?.CredentialsId ?? e.OrganizationClientId ?? e.PersonClientId)
                    .Where(e => e != null)
                    .Select(e => e.ToString())
                    .ToList();
            var recepientIdSelf =
                await _avetonDbContext.ChatMembers
                .Where(e => e.Id == newMessage.OwnerId)
                .Select(e => e.Employee!.CredentialsId ?? e.OrganizationClientId ?? e.PersonClientId)
                .FirstOrDefaultAsync();
            if (recepientIds != null)
            {
                await _signalRService.SendNewMessageAsync(
                    new SignalREventNotificationInfo()
                    {
                        EventOriginConnectionId = eventOriginConnectionId,
                        GroupNames = recepientIds!,
                        EventOriginGroupName = recepientIdSelf?.ToString() ?? string.Empty
                    }, _mapper.Map<GetChatMessageModel>(result));                
            }
            return new SuccessfullCreateModel(result);
        }

        public async Task<PageModel<GetChatMessageModel>> GetMessagesForChatAsync(Guid chatId, int startIndex = 0, int itemsPerPage = 50)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ChatMessage).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var messagesQuery = _avetonDbContext.ChatMessages.Where(e => e.ChatId == chatId && e.EntityOwnerId == _currentUserOwnerId);
            var messagesCount = await messagesQuery.CountAsync();
            var messagesResult = await messagesQuery
                    .OrderByDescending(e => e.CreatedOn)
                    .Skip(startIndex)
                    .Take(itemsPerPage)
                    .ToListAsync();
            return new PageModel<GetChatMessageModel>
            {
                Items = _mapper.Map<List<GetChatMessageModel>>(messagesResult),
                TotalItems = messagesCount,
                StartIndex = startIndex,
                ItemsPerPage = itemsPerPage,
            };
        }

        public async Task<GetChatMessageViewedInfoModel?> ViewMessageAsync(Guid messageId, Guid viewedByChatMemberId, string eventOriginConnectionId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ChatMessage).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }            
            var message = await _avetonDbContext.ChatMessages.FindAsync(messageId);
            if (message == null) 
            {
                throw new EntityNotFoundException(nameof(ChatMessage));
            }
            //нельзя прочитать своё же сообщение 
            if (message?.Owner?.EmployeeId == await _currentUserService.GetEmployeeIdForCurrentUserAsync())
            {
                throw new EntityNotFoundException(nameof(ChatMessage));
            }
            var messageViewedInfo = new ChatMessageViewedInfo();
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            messageViewedInfo.CreatedByUser = currentUserLogin;
            messageViewedInfo.UpdatedByUser = currentUserLogin;
            messageViewedInfo.EntityOwnerId = _currentUserOwnerId;
            messageViewedInfo.MessageId = messageId;
            messageViewedInfo.ViewedById = viewedByChatMemberId;
            var result = await _avetonDbContext.InsertAsync(messageViewedInfo);
            var modelResult = _mapper.Map<GetChatMessageViewedInfoModel>(result);
            var recepientIds =
                (await _avetonDbContext.Chats.FirstOrDefaultAsync(e => e.Id == message!.ChatId))
                    ?.ChatMembers
                    //Всем кроме меня
                    .Where(e => e.Id != viewedByChatMemberId)
                    .Select(e => e.Employee?.CredentialsId ?? e.OrganizationClientId ?? e.PersonClientId)
                    .Where(e => e != null)
                    .Select(e => e.ToString())
                    .ToList();
            var recepientIdSelf =
                await _avetonDbContext.ChatMembers
                .Where(e => e.Id == viewedByChatMemberId)
                .Select(e => e.Employee!.CredentialsId ?? e.OrganizationClientId ?? e.PersonClientId)
                .FirstOrDefaultAsync();
            if (recepientIds != null)
            {
                await _signalRService.SendNewMessageViewedInfoAsync(
                    new SignalREventNotificationInfo()
                    {
                        EventOriginConnectionId = eventOriginConnectionId,
                        GroupNames = recepientIds!,
                        EventOriginGroupName = recepientIdSelf?.ToString() ?? string.Empty
                    }, message!.ChatId, modelResult);
            }
            return modelResult;
        }

        public async Task<AttachFileModel> GetChatMessageAttachedFileContentAsync(Guid chatMessageAttachedFileId, bool isImageMedium = false)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ChatMessage).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbEntity = await _avetonDbContext.GetFirstOrDefaultAsync<ChatMessageAttachedFile>(x => x.Id == chatMessageAttachedFileId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbEntity == null)
            {
                throw new EntityNotFoundException(nameof(StageReportAttachedFile));
            }
            if (string.IsNullOrWhiteSpace(dbEntity.FilePath))
            {
                throw new FileContentReadingException();
            }
            AttachFileModel? result = null;
            if (isImageMedium)
            {
                if (string.IsNullOrWhiteSpace(dbEntity.ImageMediumSizeFilePath))
                {
                    throw new FileContentReadingException();
                }
                result = await _fileProviderService.GetFileDataUrlAsync(dbEntity.ImageMediumSizeFilePath);
            }
            else
            {
                result = await _fileProviderService.GetFileDataUrlAsync(dbEntity.FilePath);
            }
            result!.FileName = dbEntity.FileName;
            return result;
        }
        protected async override Task<bool> CanDeleteEntityAsync(ChatMessage entityToDelete)
        {
            return true;
        }

        protected override IQueryable<ChatMessage> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.ChatMessages
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                filterQuery = filterQuery.Where(e => e.Text!.ToLower().Contains(filterString));
            }
            return filterQuery;
        }

        protected override IQueryable<ChatMessage> OrderByConditionQuery(IQueryable<ChatMessage> query)
        {
            return query.OrderByDescending(e => e.CreatedOn).AsQueryable();
        }
    }
}
