using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.ProjectStage;

namespace Services.Implementations
{
    public class ProjectStageService : CrudService<ProjectStage, GetProjectStageModel, CreateProjectStageModel, UpdateProjectStageModel>, IProjectStageService
    {
        protected readonly IStageReportService _stageReportService;
        public ProjectStageService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService, IStageReportService stageReportService) : base(avetonDbContext, mapper, currentUserService)
        {
            _stageReportService = stageReportService;
        }

        public override async Task<SuccessfullCreateModel> CreateAsync(CreateProjectStageModel newModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ProjectStage).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var model = _mapper.Map<ProjectStage>(newModel);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            model.CreatedByUser = currentUserLogin;
            model.UpdatedByUser = currentUserLogin;
            model.EntityOwnerId = _currentUserOwnerId;
            var currentMaxOrder = await _avetonDbContext.ProjectStages
                .AsQueryable()
                .Where(e => e.ParentStageId == model.ParentStageId && e.ProjectId == model.ProjectId && e.EntityOwnerId == _currentUserOwnerId)
                .OrderByDescending(e => e.OrderNumber)
                .FirstOrDefaultAsync();
            model.OrderNumber = currentMaxOrder?.OrderNumber ?? 0 + 1;
            var result = await _avetonDbContext.InsertAsync(model);
            return new SuccessfullCreateModel(result);
        }

        public async Task<List<GetProjectStageModel>> GetAllForProjectAsync(Guid projectId, string? filterString = "")
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ProjectStage).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }

            var firstLayerDivisions =
                _avetonDbContext.ProjectStages
                    .AsQueryable()
                    .Where(e => e.ParentStageId == null && e.ProjectId == projectId && e.EntityOwnerId == _currentUserOwnerId)
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

        public async Task<string> GetProjectNameAsync(Guid entityId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ProjectStage).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<ProjectStage>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(ProjectStage));
            }
            return model.Project?.Name ?? string.Empty;
        }

        public override async Task DeleteAsync(Guid id)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(ProjectStage).Name, DataCore.Enums.EntityAction.Delete))
            {
                throw new ActionNotAllowedException();
            }
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<ProjectStage>(x => x.Id == id && x.EntityOwnerId == _currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(ProjectStage));
            }
            if (await CanDeleteEntityAsync(model))
            {
                await DeleteStageRelatedInfo(model);
                await DeleteChildStages(model);
            }
        }

        protected override async Task<bool> CanDeleteEntityAsync(ProjectStage entityToDelete)
        {
            return true;
        }

        protected override IQueryable<ProjectStage> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.ProjectStages.AsQueryable();
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

        protected override IQueryable<ProjectStage> OrderByConditionQuery(IQueryable<ProjectStage> query)
        {
            return query.OrderByDescending(e => e.Name).AsQueryable();
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
            var childs = model.ChildStages.ToList();
            model.ChildStages = new List<GetProjectStageModel>();
            foreach (var child in childs)
            {
                if (child.Name.ToLower().Contains(filterString))
                {
                    model.ChildStages.Add(child);
                }
                else if (ChildNamesContainsFilter(child.ChildStages.ToList(), filterString))
                {
                    var childProgress = child.CurrentProgress;
                    FilterChilds(child, filterString);
                    child.CurrentProgress = childProgress;
                    model.ChildStages.Add(child);                    
                }
            }

        }

        private async Task DeleteChildStages(ProjectStage projectStage)
        {
            foreach (var childStage in projectStage.ChildStages.ToList())
            {
                await DeleteStageRelatedInfo(childStage);
                await DeleteChildStages(childStage);
            }
            await _avetonDbContext.DeleteAsync(projectStage);
        }

        private async Task DeleteStageRelatedInfo(ProjectStage projectStage)
        {            
            foreach(var report in projectStage.StageReports.ToList())
            {
                await _stageReportService.DeleteAsync(report.Id);
            }
        }
    }
}
