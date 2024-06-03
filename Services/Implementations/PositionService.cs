using AutoMapper;
using DataCore.Entities;
using DataProvider;
using Services.Interfaces;
using Services.Models.Position;
using System.Data;

namespace Services.Implementations
{
    public class PositionService : CrudService<Position, GetPositionModel, CreatePositionModel, UpdatePositionModel>, IPositionService
    {
        public PositionService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        protected override async Task<bool> CanDeleteEntityAsync(Position entityToDelete)
        {
            if (entityToDelete.Jobs.Any())
            {
                throw new Exception("Невозможно удалить должность, так как она связана с местом работы.");
            }
            return true;
        }

        protected override IQueryable<Position> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Positions.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                filterQuery = filterQuery.Where(e =>
                    e.Name.ToLower().Contains(filterString)
                    );
            }
            return filterQuery;
        }

        protected override IQueryable<Position> OrderByConditionQuery(IQueryable<Position> query)
        {
            return query.OrderBy(e => e.Name).AsQueryable();
        }
    }
}
