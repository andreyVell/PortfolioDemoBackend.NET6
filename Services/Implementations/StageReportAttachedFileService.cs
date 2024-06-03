using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.StageReportAttachedFile;

namespace Services.Implementations
{
    public class StageReportAttachedFileService
        : IStageReportAttachedFileService
    {
        protected readonly IAvetonDbContext _avetonDbContext;
        protected readonly IMapper _mapper;
        protected readonly ICurrentUserDataService _currentUserService;
        protected readonly IFileProviderService _fileProviderService;
        protected readonly Guid _currentUserOwnerId;

        public StageReportAttachedFileService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService, IFileProviderService fileProviderService)
        {
            _avetonDbContext = avetonDbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _fileProviderService = fileProviderService;
            _currentUserOwnerId = _currentUserService.GetCurrentUserOwnerId();
        }

        public async Task<SuccessfullCreateModel> CreateAsync(CreateStageReportAttachedFileModel model)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(StageReportAttachedFile).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            var attachedFile = new StageReportAttachedFile();
            attachedFile.CreatedByUser = currentUserLogin;
            attachedFile.UpdatedByUser = currentUserLogin;
            attachedFile.FileName = model.File?.FileName;
            attachedFile.StageReportId = model.StageReportId;
            attachedFile.EntityOwnerId = _currentUserOwnerId;
            attachedFile.FilePath = await _fileProviderService.SaveFileAsync(model.File!);
            if (ImageFormatter.IsBase64StringIsImage(model.File!.FileContent))
            {
                attachedFile.ImageMediumSizeFilePath = await _fileProviderService.CompressAndSaveImageAsync(model.File!, 600, 900);
            }
            var result = await _avetonDbContext.InsertAsync(attachedFile);
            return new SuccessfullCreateModel(result);
        }

        public async Task DeleteAsync(Guid entityId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(StageReportAttachedFile).Name, DataCore.Enums.EntityAction.Delete))
            {
                throw new ActionNotAllowedException();
            }
            var dbEntity = await _avetonDbContext.GetFirstOrDefaultAsync<StageReportAttachedFile>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbEntity == null)
            {
                throw new EntityNotFoundException(nameof(StageReportAttachedFile));
            }
            if (await CanDeleteEntityAsync(dbEntity))
            {
                if (!string.IsNullOrWhiteSpace(dbEntity.FilePath))
                {
                    await _fileProviderService.DeleteAsync(dbEntity.FilePath);
                }
                if (!string.IsNullOrWhiteSpace(dbEntity.ImageMediumSizeFilePath))
                {
                    await _fileProviderService.DeleteAsync(dbEntity.ImageMediumSizeFilePath);
                }
                await _avetonDbContext.DeleteAsync(dbEntity);
            }
        }

        public async Task<AttachFileModel> GetFileContentAsync(Guid entityId, bool isImageMedium = false)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(StageReportAttachedFile).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbEntity = await _avetonDbContext.GetFirstOrDefaultAsync<StageReportAttachedFile>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId);
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

        protected async Task<bool> CanDeleteEntityAsync(StageReportAttachedFile entityToDelete)
        {
            return true;
        }
    }
}
