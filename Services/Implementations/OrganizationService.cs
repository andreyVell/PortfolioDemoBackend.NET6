using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Organization;

namespace Services.Implementations
{
    public class OrganizationService : CrudService<Organization, GetOrganizationModel, CreateOrganizationModel, UpdateOrganizationModel>, IOrganizationService
    {
        public OrganizationService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        public override async Task<SuccessfullCreateModel> CreateAsync(CreateOrganizationModel newModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Organization).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var existedOrganization = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<Organization>(x => x.Login == newModel.Login);
            if (existedOrganization != null)
            {
                throw new ClientAlreadyExistsException();
            }
            var model = _mapper.Map<Organization>(newModel);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            model.CreatedByUser = currentUserLogin;
            model.UpdatedByUser = currentUserLogin;
            model.EntityOwnerId = _currentUserOwnerId;
            var result = await _avetonDbContext.InsertAsync(model);
            return new SuccessfullCreateModel(result);
        }

        protected override async Task<bool> CanDeleteEntityAsync(Organization entityToDelete)
        {
            return true;
        }

        protected override IQueryable<Organization> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Organizations.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();                
                filterQuery = filterQuery.Where(e =>
                e.Name.ToLower().Contains(filterString)
                || e.Inn.ToLower().Contains(filterString)
                || e.ContactEmail.ToLower().Contains(filterString)
                || e.ContactPhone.ToLower().Contains(filterString)
                || e.Login.ToLower().Contains(filterString)
                );
            }
            return filterQuery;
        }

        protected override IQueryable<Organization> OrderByConditionQuery(IQueryable<Organization> query)
        {
            return query.OrderBy(e => e.Name).AsQueryable();
        }
    }
}
