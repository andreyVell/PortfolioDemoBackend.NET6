using AutoMapper;
using DataCore.Entities;
using DataProvider;
using Services.Interfaces;
using Services.Models.DivisionContractor;

namespace Services.Implementations
{
    public class DivisionContractorService : CrudService<DivisionContractor, GetDivisionContractorModel, CreateDivisionContractorModel, UpdateDivisionContractorModel>, IDivisionContractorService
    {
        public DivisionContractorService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        protected override async Task<bool> CanDeleteEntityAsync(DivisionContractor entityToDelete)
        {
            return true;
        }

        protected override IQueryable<DivisionContractor> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.DivisionContractors.AsQueryable();            
            return filterQuery;
        }

        protected override IQueryable<DivisionContractor> OrderByConditionQuery(IQueryable<DivisionContractor> query)
        {
            return query.OrderByDescending(e => e.Id).AsQueryable();
        }
    }
}
