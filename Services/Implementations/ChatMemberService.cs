using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.ChatMember;

namespace Services.Implementations
{
    public class ChatMemberService : CrudService<ChatMember, GetChatMemberModel, CreateChatMemberModel, UpdateChatMemberModel>, IChatMemberService
    {
        protected readonly IFileProviderService _fileProviderService;

        public ChatMemberService(
            IAvetonDbContext avetonDbContext, 
            IMapper mapper, 
            ICurrentUserDataService currentUserService,
            IFileProviderService fileProviderService) : base(avetonDbContext, mapper, currentUserService)
        {
            _fileProviderService = fileProviderService;
        }

        public async Task<AttachFileModel?> GetChatMemberSmallAvatarAsync(Guid chatMemberId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbChatMember = await _avetonDbContext.GetFirstOrDefaultAsync<ChatMember>(x => x.Id == chatMemberId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbChatMember == null)
            {
                throw new EntityNotFoundException(nameof(ChatMember));
            }
            //Chats: only employee avatar
            return await _fileProviderService.GetFileDataUrlAsync(dbChatMember?.Employee?.PathToSmallAvatar);

        }

        public async Task<PageModel<GetChatMemberModel>> GetPotentialChatMembersForChatAsync(int startIndex = 0, int itemsPerPage = 50, string? filterString = "", Guid? chatId = null)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Chat).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            //вернуть всех эмплойеров(кроме тебя) и Persons and Organization 
            var currentUserEmployeeId = await _currentUserService.GetEmployeeIdForCurrentUserAsync();
            var chatMembers = await _avetonDbContext.Chats
                .Where(e => e.Id == chatId && e.EntityOwnerId == _currentUserOwnerId)
                .SelectMany(e => e.ChatMembers)
                .ToListAsync() ?? new List<ChatMember>();

            var filterQueryEmployees =
                GetFilterQueryForEmployees(filterString!, 
                    chatMembers.Where(e => e.Type == DataCore.Enums.ChatMemberType.Employee)
                    .Select(e => e.EmployeeId ?? Guid.Empty))
                .Where(x => x.EntityOwnerId == _currentUserOwnerId && x.Id != currentUserEmployeeId);
            var totalItemsEmployees = await filterQueryEmployees.CountAsync();
            filterQueryEmployees = OrderByConditionQuery(filterQueryEmployees);

            var filterQueryPersons =
               GetFilterQueryForPersons(filterString!,
                    chatMembers.Where(e => e.Type == DataCore.Enums.ChatMemberType.PersonClient)
                    .Select(e => e.PersonClientId ?? Guid.Empty))
               .Where(x => x.EntityOwnerId == _currentUserOwnerId);
            var totalItemsPersons = await filterQueryPersons.CountAsync();
            filterQueryPersons = OrderByConditionQuery(filterQueryPersons);

            var filterQueryOrganizations =
               GetFilterQueryForOrganizations(filterString!,
                    chatMembers.Where(e => e.Type == DataCore.Enums.ChatMemberType.OrganizationClient)
                    .Select(e => e.OrganizationClientId ?? Guid.Empty))
               .Where(x => x.EntityOwnerId == _currentUserOwnerId);
            var totalItemsOrganizations = await filterQueryOrganizations.CountAsync();
            filterQueryOrganizations = OrderByConditionQuery(filterQueryOrganizations);

            filterQueryEmployees = filterQueryEmployees
                    .Skip(startIndex)
                    .Take(itemsPerPage);
            var pageQueryEmployees = await filterQueryEmployees.ToListAsync();

            //если хватает только сотрудников то их и отсылаем, если нет до добавляем следующие
            var resultList = pageQueryEmployees.Select(e => new GetChatMemberModel()
            {
                Type = DataCore.Enums.ChatMemberType.Employee,
                EmployeeId = e.Id,
                Employee = _mapper.Map<GetChatEmployeeModel>(e),
            }).ToList();

            if (resultList.Count == itemsPerPage)
            {
                //сотрудников хватает берём только их 
                return new PageModel<GetChatMemberModel>
                {
                    Items = resultList,
                    TotalItems = totalItemsEmployees + totalItemsPersons + totalItemsOrganizations,
                    StartIndex = startIndex,
                    ItemsPerPage = itemsPerPage,
                };
            }
            //только сотрудников не хватает, надо ещё            
            filterQueryPersons = filterQueryPersons
                .Skip(startIndex >= totalItemsEmployees ? startIndex - totalItemsEmployees : 0)
                .Take(itemsPerPage - resultList.Count);
            var pageQueryPersons = await filterQueryPersons.ToListAsync();
            resultList.AddRange(
                   pageQueryPersons.Select(e => new GetChatMemberModel()
                   {
                       Type = DataCore.Enums.ChatMemberType.PersonClient,
                       PersonClientId = e.Id,
                       PersonClient = _mapper.Map<GetChatPersonModel>(e),
                   }));
            if (resultList.Count == itemsPerPage)
            {
                return new PageModel<GetChatMemberModel>
                {
                    Items = resultList,
                    TotalItems = totalItemsEmployees + totalItemsPersons + totalItemsOrganizations,
                    StartIndex = startIndex,
                    ItemsPerPage = itemsPerPage,
                };
            }
            //сотрудников + физ лиц не хватает
            filterQueryOrganizations = filterQueryOrganizations
                    .Skip(startIndex >= (totalItemsEmployees + totalItemsPersons) ? startIndex - totalItemsEmployees - totalItemsPersons : 0)
                    .Take(itemsPerPage - resultList.Count);
            var pageQueryOrganizations = await filterQueryOrganizations.ToListAsync();
            resultList.AddRange(
                    pageQueryOrganizations.Select(e => new GetChatMemberModel()
                    {
                        Type = DataCore.Enums.ChatMemberType.OrganizationClient,
                        OrganizationClientId = e.Id,
                        OrganizationClient = _mapper.Map<GetChatOrganizationModel>(e),
                    }));

            return new PageModel<GetChatMemberModel>
            {
                Items = resultList,
                TotalItems = totalItemsEmployees + totalItemsPersons + totalItemsOrganizations,
                StartIndex = startIndex,
                ItemsPerPage = itemsPerPage,
            };
        }
        

        protected async override Task<bool> CanDeleteEntityAsync(ChatMember entityToDelete)
        {
            return true;
        }

        protected override IQueryable<ChatMember> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.ChatMembers
                .AsQueryable();            
            return filterQuery;
        }

        protected override IQueryable<ChatMember> OrderByConditionQuery(IQueryable<ChatMember> query)
        {
            return query.OrderByDescending(e => e.CreatedOn).AsQueryable();
        }


        private IQueryable<Employee> GetFilterQueryForEmployees(string filterString, IEnumerable<Guid> excludedEmployeesIds)
        {
            var filterQuery = _avetonDbContext.Employees.AsQueryable().Where(e => !excludedEmployeesIds.Any(x => x == e.Id));
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                if (filterString.Split(' ').Length == 1)
                {
                    if (DateTime.TryParse(filterString, out var date))
                    {
                        filterQuery = filterQuery.Where(e =>
                        e.FirstName!.ToLower().Contains(filterString)
                        || e.LastName!.ToLower().Contains(filterString)
                        || e.SecondName!.ToLower().Contains(filterString)
                        || e.Email!.ToLower().Contains(filterString)
                        || e.MobilePhoneNumber!.ToLower().Contains(filterString)
                        || e.LastJobProjectable!.Division!.Name!.ToLower().Contains(filterString)
                        || e.LastJobProjectable!.Position!.Name!.ToLower().Contains(filterString)
                        || e.Credentials!.Roles.Any(e => e.Name.ToLower().Contains(filterString))
                        || (e.Birthday!.Value.Year == date.Year && e.Birthday.Value.Month == date.Month && e.Birthday.Value.Day == date.Day)
                        );
                    }
                    else
                    {
                        filterQuery = filterQuery.Where(e =>
                        e.FirstName!.ToLower().Contains(filterString)
                        || e.LastName!.ToLower().Contains(filterString)
                        || e.SecondName!.ToLower().Contains(filterString)
                        || e.Email!.ToLower().Contains(filterString)
                        || e.MobilePhoneNumber!.ToLower().Contains(filterString)
                        || e.LastJobProjectable!.Division!.Name!.ToLower().Contains(filterString)
                        || e.LastJobProjectable!.Position!.Name!.ToLower().Contains(filterString)
                        || e.Credentials!.Roles.Any(e => e.Name.ToLower().Contains(filterString))
                        );
                    }
                }
                else
                {
                    filterQuery = filterQuery.Where(e =>
                        (e.LastName + " " + e.FirstName + " " + e.SecondName + " " + e.Birthday).ToLower().Contains(filterString)
                        || e.Email!.ToLower().Contains(filterString)
                        || e.MobilePhoneNumber!.ToLower().Contains(filterString)
                        || e.LastJobProjectable!.Division!.Name!.ToLower().Contains(filterString)
                        || e.LastJobProjectable!.Position!.Name!.ToLower().Contains(filterString)
                        || e.Credentials!.Roles.Any(e => e.Name.ToLower().Contains(filterString))
                        );
                }


            }
            return filterQuery;
        }

        private IQueryable<Employee> OrderByConditionQuery(IQueryable<Employee> query)
        {
            return query.OrderBy(e => e.LastName + e.FirstName + e.SecondName).AsQueryable();
        }

        private IQueryable<Person> GetFilterQueryForPersons(string filterString, IEnumerable<Guid> excludedPersonsIds)
        {
            var filterQuery = _avetonDbContext.Persons.AsQueryable().Where(e => !excludedPersonsIds.Any(x => x == e.Id));
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                if (filterString.Split(' ').Length == 1)
                {
                    filterQuery = filterQuery.Where(e =>
                        e.FirstName!.ToLower().Contains(filterString)
                        || e.LastName!.ToLower().Contains(filterString)
                        || e.SecondName!.ToLower().Contains(filterString)
                        || e.ContactEmail!.ToLower().Contains(filterString)
                        || e.ContactPhone!.ToLower().Contains(filterString)
                        || e.Login.ToLower().Contains(filterString)
                        );
                }
                else
                {
                    filterQuery = filterQuery.Where(e =>
                        (e.LastName + " " + e.FirstName + " " + e.SecondName).ToLower().Contains(filterString)
                        || e.ContactEmail!.ToLower().Contains(filterString)
                        || e.ContactPhone!.ToLower().Contains(filterString)
                        || e.Login.ToLower().Contains(filterString)
                        );
                }


            }
            return filterQuery;
        }

        private IQueryable<Person> OrderByConditionQuery(IQueryable<Person> query)
        {
            return query.OrderBy(e => e.LastName + e.FirstName + e.SecondName).AsQueryable();
        }

        private IQueryable<Organization> GetFilterQueryForOrganizations(string filterString, IEnumerable<Guid> excludedOrganizationsIds)
        {
            var filterQuery = _avetonDbContext.Organizations.AsQueryable().Where(e => !excludedOrganizationsIds.Any(x => x == e.Id));
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();
                filterQuery = filterQuery.Where(e =>
                e.Name!.ToLower().Contains(filterString)
                || e.Inn!.ToLower().Contains(filterString)
                || e.ContactEmail!.ToLower().Contains(filterString)
                || e.ContactPhone!.ToLower().Contains(filterString)
                || e.Login.ToLower().Contains(filterString)
                );
            }
            return filterQuery;
        }

        private IQueryable<Organization> OrderByConditionQuery(IQueryable<Organization> query)
        {
            return query.OrderBy(e => e.Name).AsQueryable();
        }
    }
}
