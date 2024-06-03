using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.StageReport;

namespace Services.Implementations
{
    public class StageReportService : CrudService<StageReport, GetStageReportModel, CreateStageReportModel, UpdateStageReportModel>, IStageReportService
    {
        protected readonly IFileProviderService _fileProviderService;
        public StageReportService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService, IFileProviderService fileProviderService) : base(avetonDbContext, mapper, currentUserService)
        {
            _fileProviderService = fileProviderService;
        }

        public override async Task<SuccessfullCreateModel> CreateAsync(CreateStageReportModel newModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(StageReport).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var dbEntity = _mapper.Map<StageReport>(newModel);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            dbEntity.CreatedByUser = currentUserLogin;
            dbEntity.UpdatedByUser = currentUserLogin;
            dbEntity.EntityOwnerId = _currentUserOwnerId;
            var employee = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(e => e.CredentialsId == _currentUserService.GetCurrentUserId() && e.EntityOwnerId == _currentUserOwnerId);
            dbEntity.EmployeeId = employee?.Id;
            if (employee != null)
            {
                var stageManager = (await _avetonDbContext.ProjectStages.FirstOrDefaultAsync(e => e.Id == newModel.ProjectStageId && e.EntityOwnerId == _currentUserOwnerId))?.StageManagers.FirstOrDefault(e=>e.EmployeeId == employee.Id && e.EntityOwnerId == _currentUserOwnerId);
                if (stageManager?.Id == null)
                {
                    throw new StageManagerAccessError();
                }
                dbEntity.StageManagerId = stageManager?.Id;                
            }
            var result = await _avetonDbContext.InsertAsync(dbEntity);    
            if (newModel.AttachedFiles != null)
            {
                foreach (var file in newModel.AttachedFiles)
                {
                    var attachedFile = new StageReportAttachedFile();
                    attachedFile.CreatedByUser = currentUserLogin;
                    attachedFile.UpdatedByUser = currentUserLogin;
                    attachedFile.FileName = file.FileName;
                    attachedFile.EntityOwnerId = _currentUserOwnerId;
                    attachedFile.StageReportId = result.Id;
                    attachedFile.FilePath = await _fileProviderService.SaveFileAsync(file);
                    if (ImageFormatter.IsBase64StringIsImage(file.FileContent))
                    {
                        attachedFile.ImageMediumSizeFilePath = await _fileProviderService.CompressAndSaveImageAsync(file, 600, 900);
                    }
                    await _avetonDbContext.InsertAsync(attachedFile);
                }
            }            
            return new SuccessfullCreateModel(result);
        }

        public override async Task<SuccessfullUpdateModel> UpdateAsync(UpdateStageReportModel updateModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(StageReport).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbModel = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<StageReport>(x => x.Id == updateModel.Id && x.EntityOwnerId == _currentUserOwnerId);
            if (dbModel == null)
            {
                throw new EntityNotFoundException(nameof(StageReport));
            }

            //check manager access
            var ccurrentEmployee = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(e => e.CredentialsId == _currentUserService.GetCurrentUserId() && e.EntityOwnerId == _currentUserOwnerId);            
            if (ccurrentEmployee != null)
            {
                var stageManager = (await _avetonDbContext.ProjectStages.FirstOrDefaultAsync(e => e.Id == updateModel.ProjectStageId && e.EntityOwnerId == _currentUserOwnerId))?.StageManagers.FirstOrDefault(e => e.EmployeeId == ccurrentEmployee.Id && e.EntityOwnerId == _currentUserOwnerId);
                if (stageManager?.Id == null)
                {
                    throw new StageManagerAccessError();
                }
            }
            CheckLostUpdate(dbModel, updateModel);
            var dbModelOwnerId = dbModel.EntityOwnerId;
            dbModel = _mapper.Map<StageReport>(updateModel);
            dbModel.UpdatedByUser = (await _currentUserService.GetCurrentUserAsync()).Login;
            dbModel.UpdatedOn = DateTime.UtcNow;
            dbModel.EntityOwnerId = dbModelOwnerId;
            var result = await _avetonDbContext.UpdateAsync(dbModel);
            return new SuccessfullUpdateModel(result);
        }

        public override async Task DeleteAsync(Guid entityId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(StageReport).Name, DataCore.Enums.EntityAction.Delete))
            {
                throw new ActionNotAllowedException();
            }
            var dbEntity = await _avetonDbContext.GetFirstOrDefaultAsync<StageReport>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbEntity == null)
            {
                throw new EntityNotFoundException(nameof(StageReport));
            }
            if (await CanDeleteEntityAsync(dbEntity))
            {
                var attachedFilesToDelete = dbEntity.AttachedFiles.ToList();
                await _fileProviderService.DeleteFilesAsync(attachedFilesToDelete
                        .Where(e => !string.IsNullOrWhiteSpace(e.FilePath))
                        .Select(e => e.FilePath!));
                await _fileProviderService.DeleteFilesAsync(attachedFilesToDelete
                        .Where(e => !string.IsNullOrWhiteSpace(e.ImageMediumSizeFilePath))
                        .Select(e => e.ImageMediumSizeFilePath!));
                await _avetonDbContext.DeleteRangeAsync(attachedFilesToDelete);
                await _avetonDbContext.DeleteAsync(dbEntity);
            }
        }

        protected override async Task<bool> CanDeleteEntityAsync(StageReport entityToDelete)
        {
            //check manager access
            var ccurrentEmployee = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(e => e.CredentialsId == _currentUserService.GetCurrentUserId() && e.EntityOwnerId == _currentUserOwnerId);
            if (ccurrentEmployee != null)
            {
                var stageManager = (await _avetonDbContext.ProjectStages.FirstOrDefaultAsync(e => e.Id == entityToDelete.ProjectStageId && e.EntityOwnerId == _currentUserOwnerId))?.StageManagers.FirstOrDefault(e => e.EmployeeId == ccurrentEmployee.Id && e.EntityOwnerId == _currentUserOwnerId);
                if (stageManager?.Id == null)
                {
                    throw new StageManagerAccessError();
                }
            }
            return true;
        }

        protected override IQueryable<StageReport> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.StageReports.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();                
                if (DateTime.TryParse(filterString, out var date))
                {
                    filterQuery = filterQuery.Where(e =>
                    e.Content.ToLower().Contains(filterString)                    
                    || (e.ReportDate.Year == date.Year && e.ReportDate.Month == date.Month && e.ReportDate.Day == date.Day)
                    );
                }
                else
                {
                    filterQuery = filterQuery.Where(e =>
                    e.Content.ToLower().Contains(filterString)                    
                    );
                }
            }
            return filterQuery;
        }

        protected override IQueryable<StageReport> OrderByConditionQuery(IQueryable<StageReport> query)
        {
            return query.OrderByDescending(e => e.ReportDate).AsQueryable();
        }
    }
}
