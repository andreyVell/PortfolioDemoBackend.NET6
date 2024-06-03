using AutoMapper;
using DataCore.Entities;
using DataProvider;
using Services.Interfaces;
using Services.Models.Job;

namespace Services.Implementations
{
    public class JobService : CrudService<Job, GetJobModel, CreateJobModel, UpdateJobModel>, IJobService
    {
        public JobService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        protected override async Task<bool> CanDeleteEntityAsync(Job entityToDelete)
        {
            return true;
        }

        protected override IQueryable<Job> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Jobs.AsQueryable();            
            return filterQuery;
        }

        protected override IQueryable<Job> OrderByConditionQuery(IQueryable<Job> query)
        {
            return query.OrderBy(e => e.StartDate).AsQueryable();
        }
    }
}
