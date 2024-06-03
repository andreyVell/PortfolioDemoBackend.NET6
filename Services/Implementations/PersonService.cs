using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Person;

namespace Services.Implementations
{
    public class PersonService : CrudService<Person, GetPersonModel, CreatePersonModel, UpdatePersonModel>, IPersonService
    {
        public PersonService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }


        public override async Task<SuccessfullCreateModel> CreateAsync(CreatePersonModel newModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Person).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var existedPerson = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<Person>(x => x.Login == newModel.Login);
            if (existedPerson != null)
            {
                throw new ClientAlreadyExistsException();
            }
            var model = _mapper.Map<Person>(newModel);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            model.CreatedByUser = currentUserLogin;
            model.UpdatedByUser = currentUserLogin;
            model.EntityOwnerId = _currentUserOwnerId;
            var result = await _avetonDbContext.InsertAsync(model);
            return new SuccessfullCreateModel(result);
        }

        protected override async Task<bool> CanDeleteEntityAsync(Person entityToDelete)
        {
            return true;
        }

        protected override IQueryable<Person> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Persons.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                if (filterString.Split(' ').Length == 1)
                {
                    filterQuery = filterQuery.Where(e =>
                        e.FirstName.ToLower().Contains(filterString)
                        || e.LastName.ToLower().Contains(filterString)
                        || e.SecondName.ToLower().Contains(filterString)
                        || e.ContactEmail.ToLower().Contains(filterString)
                        || e.ContactPhone.ToLower().Contains(filterString)
                        || e.Login.ToLower().Contains(filterString)
                        );
                }
                else
                {
                    filterQuery = filterQuery.Where(e =>
                        (e.LastName + " " + e.FirstName + " " + e.SecondName).ToLower().Contains(filterString)
                        || e.ContactEmail.ToLower().Contains(filterString)
                        || e.ContactPhone.ToLower().Contains(filterString)
                        || e.Login.ToLower().Contains(filterString)
                        );
                }


            }
            return filterQuery;
        }

        protected override IQueryable<Person> OrderByConditionQuery(IQueryable<Person> query)
        {
            return query.OrderBy(e => e.LastName + e.FirstName + e.SecondName).AsQueryable();
        }
    }
}
