using AutoMapper;
using DataCore.Entities;
using DataCore.Enums;
using DataCore.Exceptions;
using DataCore.Exceptions.AvetonUser;
using DataProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Models.AvetonRoleAccess;
using Services.Models.AvetonUser;
using Services.Models.Employee;
using System.Security.Claims;

namespace Services.Implementations
{
    public class CurrentUserDataService : ICurrentUserDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAvetonDbContext _avetonDbContext;
        private readonly IMapper _mapper;
        private readonly IGlobalSettings _globalSettings;

        public CurrentUserDataService(
            IGlobalSettings globalSettings,
            IHttpContextAccessor httpContextAccessor, 
            IAvetonDbContext avetonDbContext, 
            IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _avetonDbContext = avetonDbContext;
            _mapper = mapper;
            _globalSettings = globalSettings;
        }

        public async Task<GetAvetonUserModel> GetCurrentUserAsync()
        {            
            return _mapper.Map<GetAvetonUserModel>(await GetCurrentUserDBAsync());
        }

        public Guid GetCurrentUserId()
        {
            Guid result = Guid.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
            }
            return result;
        }

        public Guid GetCurrentUserOwnerId()
        {
            Guid result = Guid.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = Guid.Parse(_httpContextAccessor.HttpContext.User?.FindFirst(_globalSettings.ClaimTypeOwnerIdentifier)?.Value ?? Guid.Empty.ToString());
            }
            return result;
        }

        public async Task<bool> IsCurrentUserHasAccessToEntityAction(string? entityName, EntityAction action)
        {            
            var currentUser = await _avetonDbContext.AvetonUsers.AsNoTracking().Where(e=>e.Id == GetCurrentUserId()).Include(e=>e.Roles).ThenInclude(e=>e.Accesses).FirstOrDefaultAsync();
            if (currentUser == null) 
            { 
                return false; 
            }
            var userRoles = currentUser.Roles;
            foreach(var role in userRoles)
            {
                if (role.IsSystemAdministrator)
                {
                    return true;
                }
                var access = role.Accesses.Where(e => e.EntityName == entityName && e.EntityAction == action && e.IsAllowed).FirstOrDefault();
                if (access != null)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<GetAvetonRoleAccessModel>> GetAccessesForCurrentUserAsync(params string?[]? entityNames)
        {
            var resultList = new List<GetAvetonRoleAccessModel>();
            var entityNamesList = entityNames?.Distinct().Where(e => !string.IsNullOrWhiteSpace(e));
            if (entityNamesList == null) return resultList;

            var currentUser = await _avetonDbContext.AvetonUsers
                .AsNoTracking()
                .Where(e => e.Id == GetCurrentUserId())
                .Include(e => e.Roles).ThenInclude(e => e.Accesses)
                .FirstOrDefaultAsync();
            var entityActions = new List<EntityAction>(4) { EntityAction.Create, EntityAction.Read, EntityAction.Update, EntityAction.Delete };
            if (currentUser == null) return resultList;
            var adminRole = currentUser.Roles.FirstOrDefault(e => e.IsSystemAdministrator);
            if (adminRole!=null)
            {
                var accesses = adminRole.Accesses;                
                foreach (var entityName in entityNamesList)
                {
                    foreach (var entityAction in entityActions)
                    {
                        resultList.Add(new GetAvetonRoleAccessModel() { IsAllowed = true, EntityAction = entityAction, EntityName = entityName! });
                    }                    
                }
                return resultList;
            }

            var allUserAccess = currentUser.Roles.SelectMany(e => e.Accesses);
            foreach (var entityName in entityNamesList)
            {
                var accessesFilteredByEntityName = allUserAccess.Where(e => e.EntityName == entityName);
                foreach (var entityAction in entityActions)
                {
                    var allowedAccess = accessesFilteredByEntityName.FirstOrDefault(e => e.EntityAction == entityAction && e.IsAllowed);
                    if (allowedAccess != null)
                    {                        
                        resultList.Add(_mapper.Map<GetAvetonRoleAccessModel>(allowedAccess));
                    }
                    else
                    {
                        var notAllowedAccess = accessesFilteredByEntityName.FirstOrDefault(e => e.EntityAction == entityAction);                        
                        if (notAllowedAccess != null)
                        {
                            resultList.Add(_mapper.Map<GetAvetonRoleAccessModel>(notAllowedAccess));
                        }                        
                    }
                }                
            }
            return resultList;
        }

        public async Task<Guid?> GetEmployeeIdForCurrentUserAsync()
        {
            return (await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<Employee>(e=>e.CredentialsId == GetCurrentUserId()))?.Id;
        }        

        private async Task<AvetonUser> GetCurrentUserDBAsync()
        {
            var user = await _avetonDbContext.GetFirstOrDefaultAsync<AvetonUser>(e => e.Id == GetCurrentUserId());
            if (user == null)
            {
                throw new UserDoesNotExistException();
            }
            return user;
        }
    }
}
