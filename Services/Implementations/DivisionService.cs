using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Division;

namespace Services.Implementations
{
    public class DivisionService : CrudService<Division, GetDivisionModel, CreateDivisionModel, UpdateDivisionModel>, IDivisionService
    {
        public DivisionService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        public async Task<List<GetDivisionWithChildsModel>> GetNestedListAsync(string filterString = "")
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Division).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }

            var firstLayerDivisions =
                _avetonDbContext.Divisions
                    .AsQueryable()
                    .Where(e => e.ParentDivisionId == null && e.EntityOwnerId == _currentUserOwnerId)
                    .OrderBy(e => e.Name);

            var firstLayerItems = await firstLayerDivisions.ToListAsync();

            if (string.IsNullOrWhiteSpace(filterString))
            {   
                return _mapper.Map<List<GetDivisionWithChildsModel>>(firstLayerItems);
            }
            else
            {
                filterString = filterString.ToLower();

                var resultItems = new List<GetDivisionWithChildsModel>();

                foreach (var item in firstLayerItems)
                {
                    if (item.Name.ToLower().Contains(filterString))
                    {
                        var mappedItem = _mapper.Map<GetDivisionWithChildsModel>(item);
                        resultItems.Add(mappedItem);
                    }
                    else if (ChildNamesContainsFilter(item.ChildDivisions.ToList(), filterString))
                    {                        
                        FilterChilds(item, filterString);
                        resultItems.Add(_mapper.Map<GetDivisionWithChildsModel>(item));
                    }
                }
                return resultItems;
            }            
        }

        public async Task<PageModel<GetDivisionModel>> GetParentDivisionsAsync()
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Division).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var filterQuery = GetFilterQuery(string.Empty).Where(e=>e.ParentDivisionId == null && e.EntityOwnerId == _currentUserOwnerId);

            var totalItems = await filterQuery.CountAsync();

            var pageQuery =
                OrderByConditionQuery(filterQuery);

            var items = await pageQuery.ToListAsync();

            var result = new PageModel<GetDivisionModel>
            {
                Items = _mapper.Map<List<GetDivisionModel>>(items),
                TotalItems = totalItems,
                StartIndex = 0,
                ItemsPerPage = items.Count,
            };

            return result;
        }

        public async Task<PageModel<GetDivisionModel>> GetChildDivisionsAsync(Guid parentDivisionId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Division).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var filterQuery = GetFilterQuery(string.Empty).Where(e => e.ParentDivisionId == parentDivisionId && e.EntityOwnerId == _currentUserOwnerId);

            var totalItems = await filterQuery.CountAsync();

            var pageQuery =
                OrderByConditionQuery(filterQuery);

            var items = await pageQuery.ToListAsync();

            var result = new PageModel<GetDivisionModel>
            {
                Items = _mapper.Map<List<GetDivisionModel>>(items),
                TotalItems = totalItems,
                StartIndex = 0,
                ItemsPerPage = items.Count,
            };

            return result;
        }

        public async override Task DeleteAsync(Guid id)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Division).Name, DataCore.Enums.EntityAction.Delete))
            {
                throw new ActionNotAllowedException();
            }
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<Division>(x => x.Id == id && x.EntityOwnerId == _currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(Division));
            }
            if (await CanDeleteEntityAsync(model))
            {
                await DeleteChildDivisions(model);
            }
        }

        protected override async Task<bool> CanDeleteEntityAsync(Division entityToDelete)
        {            
            return true;
        }

        protected async Task DeleteChildDivisions(Division division)
        {
            foreach (var childDivision in division.ChildDivisions.ToList())
            {
                await DeleteChildDivisions(childDivision);
            }
            await _avetonDbContext.DeleteAsync(division);
        }

        protected override IQueryable<Division> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Divisions.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                filterQuery = filterQuery.Where(e =>
                    e.Name!.ToLower().Contains(filterString)
                    );
            }
            return filterQuery;
        }

        protected override IQueryable<Division> OrderByConditionQuery(IQueryable<Division> query)
        {
            return query.OrderBy(e => e.Name).AsQueryable();
        }

        private bool ChildNamesContainsFilter(List<Division> childs, string filterString)
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
                if (ChildNamesContainsFilter(child.ChildDivisions.ToList(), filterString))
                {
                    return true;
                }
            }
            return false;
        }        

        private void FilterChilds(Division model, string filterString)
        {
            var childs = model.ChildDivisions.ToList();
            model.ChildDivisions = new List<Division>();
            foreach (var child in childs)
            {
                if (child.Name.ToLower().Contains(filterString))
                {
                    model.ChildDivisions.Add(child);
                }
                else if (ChildNamesContainsFilter(child.ChildDivisions.ToList(), filterString))
                {
                    model.ChildDivisions.Add(child);
                    FilterChilds(child, filterString);
                }
            }
            
        }
    }
}
