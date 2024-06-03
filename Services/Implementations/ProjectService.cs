using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Project;

namespace Services.Implementations
{
    public class ProjectService : CrudService<Project, GetProjectModel, CreateProjectModel, UpdateProjectModel>, IProjectService
    {
        private readonly ISubscriptionService _subscriptionService;
        public ProjectService(
            IAvetonDbContext avetonDbContext, 
            IMapper mapper, 
            ICurrentUserDataService currentUserService, 
            ISubscriptionService subscriptionService) : base(avetonDbContext, mapper, currentUserService)
        {
            _subscriptionService = subscriptionService;
        }

        //TODO когда удалять, удалять этапы проекта через projectStageService, чтобы удалялись все созданые файлы из отчётов

        public async override Task<SuccessfullCreateModel> CreateAsync(CreateProjectModel newModel)
        {
            var projectsCount = await _avetonDbContext.Projects.Where(e => e.EntityOwnerId == _currentUserOwnerId && e.CreatedOn.Month == DateTime.UtcNow.Month).CountAsync();
            var subscriptionLimit = await _subscriptionService.GetOrganizationMaxProjectsAsync(_currentUserOwnerId);
            if (projectsCount >= subscriptionLimit)
            {
                throw new SubscriptionLimitException();
            }
            return await base.CreateAsync(newModel);            
        }

        protected override async Task<bool> CanDeleteEntityAsync(Project entityToDelete)
        {
            return true;
        }

        protected override IQueryable<Project> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Projects.AsQueryable();
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

        protected override IQueryable<Project> OrderByConditionQuery(IQueryable<Project> query)
        {
            return query.OrderByDescending(e => e.CreatedOn).AsQueryable();
        }
    }
}
