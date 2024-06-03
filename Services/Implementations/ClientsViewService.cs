using AutoMapper;
using DataCore.Entities;
using DataCore.Enums;
using DataCore.Exceptions;
using DataCore.Exceptions.AvetonUser;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Authentication;
using Services.Models.Chat;
using Services.Models.ChatMessage;
using Services.Models.Project;
using Services.Models.ProjectStage;
using Services.Models.SignalR;

namespace Services.Implementations
{
    public class ClientsViewService : IClientsViewService
    {
        private readonly IAvetonDbContext _avetonDbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserDataService _currentUserService;
        private readonly IFileProviderService _fileProviderService;
        private readonly IGlobalSettings _globalSettings;
        private readonly Guid _currentUserOwnerId;
        protected readonly ISignalRService _signalRService;

        public ClientsViewService(
            ISignalRService signalRService,
            IAvetonDbContext avetonDbContext, 
            IGlobalSettings globalSettings, 
            IMapper mapper, 
            ICurrentUserDataService currentUserService, 
            IFileProviderService fileProviderService)
        {
            _currentUserService = currentUserService;
            _mapper = mapper;
            _avetonDbContext = avetonDbContext;
            _globalSettings = globalSettings;
            _fileProviderService = fileProviderService;
            _currentUserOwnerId = _currentUserService.GetCurrentUserOwnerId();
            _signalRService = signalRService;
        }

        public async Task<string> LoginAsync(LoginClientModel user)
        {
            switch (user.ClientType)
            {
                case ClientType.Organization:
                    return await LoginAsOrganization(user);
                case ClientType.Person:
                    return await LoginAsPerson(user);
            }
            throw new UserLoginIncorrectDataException();
        }

        public virtual async Task<PageModel<GetProjectModel>> GetProjectPageForCurrentUserAsync(int startIndex = 0, int itemsPerPage = 50, string filterString = "")
        {
            var filterQuery = GetFilterQueryProject(filterString).Where(e => e.EntityOwnerId == _currentUserOwnerId);

            var totalItems = await filterQuery.CountAsync();

            var pageQuery =
                OrderByConditionQueryProject(filterQuery)
                .Skip(startIndex)
                .Take(itemsPerPage);

            var result = new PageModel<GetProjectModel>
            {
                Items = _mapper.Map<List<GetProjectModel>>(await pageQuery.ToListAsync()),
                TotalItems = totalItems,
                StartIndex = startIndex,
                ItemsPerPage = itemsPerPage,
            };

            return result;
        }

        public virtual async Task<GetProjectModel> GetProjectDetailsAsync(Guid entityId)
        {            
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();
            var dbEntity = await _avetonDbContext.Projects.AsQueryable()
                .Where(e => e.Clients
                    .Any(a => a.OrganizationId == currentOrganizationOrPersonId || a.PersonId == currentOrganizationOrPersonId))
                .FirstOrDefaultAsync(e => e.Id == entityId && e.EntityOwnerId == _currentUserOwnerId);
            if (dbEntity == null) 
            {
                throw new EntityNotFoundException(nameof(Project));
            }
            return _mapper.Map<GetProjectModel>(dbEntity);
        }

        public async Task<List<GetProjectStageModel>> GetAllStagesForProjectAsync(Guid projectId, string? filterString = "")
        {
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();
            var dbProject = await _avetonDbContext.Projects.AsQueryable()
                .Where(e => e.Clients
                    .Any(a => a.OrganizationId == currentOrganizationOrPersonId || a.PersonId == currentOrganizationOrPersonId))
                .FirstOrDefaultAsync(e => e.Id == projectId && e.EntityOwnerId == _currentUserOwnerId);
            if (dbProject == null)
            {
                throw new EntityNotFoundException(nameof(Project));
            }

            var firstLayerDivisions =
                _avetonDbContext.ProjectStages
                    .AsQueryable()
                    .Where(e => e.ParentStageId == null && e.ProjectId == dbProject.Id && e.EntityOwnerId == _currentUserOwnerId)
                    .OrderBy(e => e.Name);

            var firstLayerItems = await firstLayerDivisions.ToListAsync();

            if (string.IsNullOrWhiteSpace(filterString))
            {
                return _mapper.Map<List<GetProjectStageModel>>(firstLayerItems);
            }
            else
            {
                filterString = filterString.ToLower();

                var resultItems = new List<GetProjectStageModel>();

                foreach (var item in firstLayerItems)
                {
                    if (item.Name.ToLower().Contains(filterString))
                    {
                        var mappedItem = _mapper.Map<GetProjectStageModel>(item);
                        resultItems.Add(mappedItem);
                    }
                    else if (ChildNamesContainsFilter(item.ChildStages.ToList(), filterString))
                    {
                        var itemProgress = item.CurrentProgress;
                        var mappedItem = _mapper.Map<GetProjectStageModel>(item);
                        FilterChilds(mappedItem, filterString);
                        mappedItem.CurrentProgress = itemProgress;
                        resultItems.Add(mappedItem);
                    }
                }
                return resultItems;
            }
        }

        public async Task<string> GetProjectNameForStageAsync(Guid projectStageId)
        {            
            var dbStage = await _avetonDbContext.GetFirstOrDefaultAsync<ProjectStage>(x => x.Id == projectStageId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbStage == null)
            {
                throw new EntityNotFoundException(nameof(ProjectStage));
            }
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();            
            var dbProject = dbStage.Project;
            if (dbProject == null) throw new EntityNotFoundException(nameof(Project));
            if (dbProject.Clients.Any(a => a.OrganizationId == currentOrganizationOrPersonId || a.PersonId == currentOrganizationOrPersonId)) 
            {
                return dbProject.Name;
            }
            else
            {
                throw new EntityNotFoundException(nameof(Project));
            }
            
        }

        public virtual async Task<GetProjectStageModel> GetProjectStageDetailsAsync(Guid projectStageId)
        {

            var dbStage = await _avetonDbContext.GetFirstOrDefaultAsync<ProjectStage>(x => x.Id == projectStageId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbStage == null)
            {
                throw new EntityNotFoundException(nameof(ProjectStage));
            }
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();
            var dbProject = dbStage.Project;
            if (dbProject == null) throw new EntityNotFoundException(nameof(Project));
            if (dbProject.Clients.Any(a => a.OrganizationId == currentOrganizationOrPersonId || a.PersonId == currentOrganizationOrPersonId))
            {
                return _mapper.Map<GetProjectStageModel>(dbStage);
            }
            else
            {
                throw new EntityNotFoundException(nameof(ProjectStage));
            }            
        }

        public async Task<AttachFileModel> GetStageReportAttachedFileContentAsync(Guid stageReportAttachedFileId, bool isImageMedium = false)
        {            
            var dbEntity = await _avetonDbContext.GetFirstOrDefaultAsync<StageReportAttachedFile>(x => x.Id == stageReportAttachedFileId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbEntity == null)
            {
                throw new EntityNotFoundException(nameof(StageReportAttachedFile));
            }
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();
            var dbProject = dbEntity.StageReport?.ProjectStage?.Project;
            if (dbProject == null) throw new EntityNotFoundException(nameof(Project));
            if (!dbProject.Clients.Any(a => a.OrganizationId == currentOrganizationOrPersonId || a.PersonId == currentOrganizationOrPersonId))
            {
                throw new EntityNotFoundException(nameof(Project));
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

        public async Task<AttachFileModel?> GetChatSmallAvatarAsync(Guid chatId)
        {            
            var dbChat = await _avetonDbContext.GetFirstOrDefaultAsync<Chat>(x => x.Id == chatId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbChat == null)
            {
                throw new EntityNotFoundException(nameof(Chat));
            }
            return await _fileProviderService.GetFileDataUrlAsync(dbChat.PathToAvatarSmallImage);
        }

        public async Task<PageModel<GetChatModel>> GetChatsPageAsync(int startIndex = 0, int itemsPerPage = 50, string filterString = "")
        {
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();
            var filterQuery = GetFilterQuery(filterString)
                .Where(x => x.EntityOwnerId == _currentUserOwnerId
                    && (x.LastMessageProjectable != null || x.IsGroupChat == true)
                    && x.ChatMembers.Any(e => e.OrganizationClientId == currentOrganizationOrPersonId || e.PersonClientId == currentOrganizationOrPersonId));

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

        public async Task<GetChatModel> GetChatAsync(Guid entityId)
        {
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<Chat>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId
                && x.ChatMembers.Any(e => e.OrganizationClientId == currentOrganizationOrPersonId || e.PersonClientId == currentOrganizationOrPersonId));
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(Chat));
            }
            var mappedChat = _mapper.Map<GetChatModel>(model);
            mappedChat.Messages = _mapper.Map<List<GetChatMessageModel>>(model!.Messages.OrderByDescending(e => e.CreatedOn));
            return mappedChat;
        }

        public async Task<SuccessfullCreateModel> CreateMessageAsync(CreateChatMessageModel newChatMessageModel, string eventOriginConnectionId)
        {
            var chatMemberSelf = await _avetonDbContext.ChatMembers.FindAsync(newChatMessageModel.OwnerId);
            if (chatMemberSelf == null)
            {
                throw new EntityNotFoundException(nameof(ChatMember));
            }
            var newMessage = _mapper.Map<ChatMessage>(newChatMessageModel);
            var chatMemberSelfLogin =
                (chatMemberSelf?.Type.ToString() ?? string.Empty)
                + (chatMemberSelf?.OrganizationClientId?.ToString() ?? chatMemberSelf?.PersonClientId?.ToString() ?? string.Empty);
            newMessage.CreatedByUser = chatMemberSelfLogin;
            newMessage.UpdatedByUser = chatMemberSelfLogin;
            newMessage.EntityOwnerId = _currentUserOwnerId;
            var result = await _avetonDbContext.InsertAsync(newMessage);
            if (newChatMessageModel.AttachFiles != null)
            {
                foreach (var file in newChatMessageModel.AttachFiles)
                {
                    var chatMessageAttachFile = new ChatMessageAttachedFile();
                    chatMessageAttachFile.CreatedByUser = chatMemberSelfLogin;
                    chatMessageAttachFile.UpdatedByUser = chatMemberSelfLogin;
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

        public async Task<AttachFileModel?> GetChatMessageAttachedFileContentAsync(Guid chatMessageAttachedFileId, bool isImageMedium = false)
        {            
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

        public async Task<PageModel<GetChatMessageModel>> GetMessagesForChatAsync(Guid chatId, int startIndex = 0, int itemsPerPage = 50)
        {            
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
            var message = await _avetonDbContext.ChatMessages.FindAsync(messageId);
            if (message == null)
            {
                throw new EntityNotFoundException(nameof(ChatMessage));
            }
            //нельзя прочитать своё же сообщение 
            var currentUserId =_currentUserService.GetCurrentUserId();
            if (message?.Owner?.OrganizationClientId == currentUserId || message?.Owner?.PersonClientId == currentUserId)
            {
                throw new EntityNotFoundException(nameof(ChatMessage));
            }
            var chatMemberSelf = await _avetonDbContext.ChatMembers.FindAsync(viewedByChatMemberId);
            if (chatMemberSelf == null)
            {
                throw new EntityNotFoundException(nameof(ChatMember));
            }
            var messageViewedInfo = new ChatMessageViewedInfo();            
            var chatMemberSelfLogin = 
                (chatMemberSelf?.Type.ToString() ?? string.Empty) 
                + (chatMemberSelf?.OrganizationClientId?.ToString() ?? chatMemberSelf?.PersonClientId?.ToString() ?? string.Empty);
            messageViewedInfo.CreatedByUser = chatMemberSelfLogin;
            messageViewedInfo.UpdatedByUser = chatMemberSelfLogin;
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
                chatMemberSelf?.Employee?.CredentialsId ?? chatMemberSelf?.OrganizationClientId ?? chatMemberSelf?.PersonClientId;
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

        public async Task<AttachFileModel?> GetChatMemberSmallAvatarAsync(Guid chatMemberId)
        {            
            var dbChatMember = await _avetonDbContext.GetFirstOrDefaultAsync<ChatMember>(x => x.Id == chatMemberId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbChatMember == null)
            {
                throw new EntityNotFoundException(nameof(ChatMember));
            }
            //Chats: only employee avatar
            return await _fileProviderService.GetFileDataUrlAsync(dbChatMember?.Employee?.PathToSmallAvatar);
        }


        private IQueryable<Chat> GetFilterQuery(string filterString)
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

        private IQueryable<Chat> OrderByConditionQuery(IQueryable<Chat> query)
        {
            return query.OrderByDescending(e => e.LastMessageProjectable!.CreatedOn).AsQueryable();
        }


        private bool ChildNamesContainsFilter(List<ProjectStage> childs, string filterString)
        {
            foreach (var child in childs)
            {
                if (child.Name.ToLower().Contains(filterString))
                {
                    return true;
                }
            }
            foreach (var child in childs)
            {
                if (ChildNamesContainsFilter(child.ChildStages.ToList(), filterString))
                {
                    return true;
                }
            }
            return false;
        }
        private bool ChildNamesContainsFilter(List<GetProjectStageModel> childs, string filterString)
        {
            foreach (var child in childs)
            {
                if (child.Name.ToLower().Contains(filterString))
                {
                    return true;
                }
            }
            foreach (var child in childs)
            {
                if (ChildNamesContainsFilter(child.ChildStages.ToList(), filterString))
                {
                    return true;
                }
            }
            return false;
        }

        private void FilterChilds(GetProjectStageModel model, string filterString)
        {
            var childs = model.ChildStages!.ToList();
            model.ChildStages = new List<GetProjectStageModel>();
            foreach (var child in childs)
            {
                if (child.Name.ToLower().Contains(filterString))
                {
                    model.ChildStages.Add(child);
                }
                else if (ChildNamesContainsFilter(child.ChildStages!.ToList(), filterString))
                {
                    var childProgress = child.CurrentProgress;
                    FilterChilds(child, filterString);
                    child.CurrentProgress = childProgress;
                    model.ChildStages.Add(child);
                }
            }

        }



        private async Task<string> LoginAsOrganization(LoginClientModel user)
        {
            var dbOrg = await _avetonDbContext.GetFirstOrDefaultAsync<Organization>(e => e.Login == user.Login && e.Password == user.Password);

            if (dbOrg != null && _globalSettings.ClientLoginDurationInHours > 0)
            {
                string token = Encryption.CreateClientOrganizationToken(dbOrg, _globalSettings.ClientLoginDurationInHours);
                return token;
            }
            else
            {
                throw new UserLoginIncorrectDataException();
            }
        }

        private async Task<string> LoginAsPerson(LoginClientModel user)
        {
            var dbPerson = await _avetonDbContext.GetFirstOrDefaultAsync<Person>(e => e.Login == user.Login && e.Password == user.Password);
            if (dbPerson != null && _globalSettings.ClientLoginDurationInHours > 0)
            {
                string token = Encryption.CreateClientPersonToken(dbPerson, _globalSettings.ClientLoginDurationInHours);
                return token;
            }
            else
            {
                throw new UserLoginIncorrectDataException();
            }
        }

        private IQueryable<Project> GetFilterQueryProject(string filterString)
        {
            var currentOrganizationOrPersonId = _currentUserService.GetCurrentUserId();
            var filterQuery = _avetonDbContext.Projects.AsQueryable()
                .Where(e => e.Clients
                    .Any(a => a.OrganizationId == currentOrganizationOrPersonId || a.PersonId == currentOrganizationOrPersonId));
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                filterQuery = filterQuery.Where(e =>
                    e.Name.ToLower().Contains(filterString)
                    || e.Description.ToLower().Contains(filterString)
                    );
            }
            return filterQuery;
        }

        private IQueryable<Project> OrderByConditionQueryProject(IQueryable<Project> query)
        {
            return query.OrderByDescending(e => e.CreatedOn).AsQueryable();
        }
    }
}
