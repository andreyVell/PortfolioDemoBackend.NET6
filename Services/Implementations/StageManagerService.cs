using AutoMapper;
using DataCore.Entities;
using DataProvider;
using Services.Interfaces;
using Services.Models.StageManager;

namespace Services.Implementations
{
    public class StageManagerService : CrudService<StageManager, GetStageManagerModel, CreateStageManagerModel, UpdateStageManagerModel>, IStageManagerService
    {
        public StageManagerService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        protected override async Task<bool> CanDeleteEntityAsync(StageManager entityToDelete)
        {
            return true;
        }

        protected override IQueryable<StageManager> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.StageManagers.AsQueryable();
            return filterQuery;
        }

        protected override IQueryable<StageManager> OrderByConditionQuery(IQueryable<StageManager> query)
        {
            return query.OrderByDescending(e => e.Id).AsQueryable();
        }
    }
}
