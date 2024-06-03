using AutoMapper;
using DataCore.Entities;
using DataCore.Enums;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.AvetonRole;

namespace Services.Implementations
{
    public class AvetonRoleService : CrudService<AvetonRole, GetAvetonRoleModel, CreateAvetonRoleModel, UpdateAvetonRoleModel>, IAvetonRoleService
    {
        public AvetonRoleService(IAvetonDbContext avetonDbContext, IMapper mapper, ICurrentUserDataService currentUserService) : base(avetonDbContext, mapper, currentUserService)
        {
        }

        public override async Task<SuccessfullUpdateModel> UpdateAsync(UpdateAvetonRoleModel updateModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(AvetonRole).Name, EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbModel = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<AvetonRole>(x => x.Id == updateModel.Id && x.EntityOwnerId == _currentUserOwnerId);
            if (dbModel == null)
            {
                throw new EntityNotFoundException(nameof(AvetonRole));
            }
            CheckLostUpdate(dbModel, updateModel);
            if (!updateModel.IsSystemAdministrator && dbModel.IsSystemAdministrator 
                && (await _avetonDbContext.GetAllAsNoTrackingAsync<Employee>(e => e.Credentials.Roles.Any(e => e.IsSystemAdministrator) && e.EntityOwnerId == _currentUserOwnerId)).Count <= 1)
            {
                throw new LastFullAccessRoleException();
            }
            var dbModelOwnerId = dbModel.EntityOwnerId;
            dbModel = _mapper.Map<AvetonRole>(updateModel);
            dbModel.UpdatedByUser = (await _currentUserService.GetCurrentUserAsync()).Login;
            dbModel.UpdatedOn = DateTime.UtcNow;
            dbModel.EntityOwnerId = dbModelOwnerId;
            var result = await _avetonDbContext.UpdateAsync(dbModel);

            foreach(var updateAccess in updateModel.Accesses)
            {
                var dbAccess = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<AvetonRoleAccess>(x => x.Id == updateAccess.Id && x.EntityOwnerId == _currentUserOwnerId);
                if (dbAccess == null)
                {
                    throw new EntityNotFoundException(nameof(AvetonRoleAccess));
                }
                var dbAccessOwnerId = dbAccess.EntityOwnerId;
                dbAccess = _mapper.Map<AvetonRoleAccess>(updateAccess);
                dbAccess.UpdatedByUser = (await _currentUserService.GetCurrentUserAsync()).Login;
                dbAccess.UpdatedOn = DateTime.UtcNow;
                dbAccess.RoleId = result.Id;
                dbAccess.EntityOwnerId = dbAccessOwnerId;
                await _avetonDbContext.UpdateAsync(dbAccess);
            }

            if (dbModel.IsDefault ?? false)
            {
                //всем пользователям её навесить
                var userDefaultRoles = new List<AvetonUsersRoles>();
                var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
                foreach (var employee in await _avetonDbContext.Employees.Where(e => !e.Credentials.Roles.Any(x => x.Id == dbModel.Id) && e.EntityOwnerId == _currentUserOwnerId).ToListAsync())
                {
                    if (employee.CredentialsId.HasValue)
                    {
                        var userRole = new AvetonUsersRoles();
                        userRole.CreatedByUser = currentUserLogin;
                        userRole.UpdatedByUser = currentUserLogin;
                        userRole.UserId = employee.CredentialsId.Value;
                        userRole.RoleId = dbModel.Id;
                        userRole.EntityOwnerId = _currentUserOwnerId;
                        userDefaultRoles.Add(userRole);
                    }
                }
                await _avetonDbContext.InsertRangeAsync(userDefaultRoles);
            }

            
            return new SuccessfullUpdateModel(result);
        }

        public override async Task<SuccessfullCreateModel> CreateAsync(CreateAvetonRoleModel newModel)
        {
            var result =  await base.CreateAsync(newModel);
            
            var baseType = typeof(EntityBase);
            var excludeTypes = new List<Type>() { typeof(AvetonRoleAccess), typeof(AvetonUsersRoles), typeof(SystemOwner) };
            var assembly = baseType.Assembly;

            var entityTypes = assembly.GetTypes().Where(t => t != baseType && !excludeTypes.Contains(t) && baseType.IsAssignableFrom(t)).ToList();
            entityTypes.Add(typeof(AvetonUser));

            var entityActions = new List<EntityAction>(4) { EntityAction.Create, EntityAction.Read, EntityAction.Update, EntityAction.Delete };
            foreach (var entityType in entityTypes)
            {
                foreach(var entityAction in entityActions)
                {
                    var avetonRoleAccess = new AvetonRoleAccess();
                    var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
                    avetonRoleAccess.CreatedByUser = currentUserLogin;
                    avetonRoleAccess.UpdatedByUser = currentUserLogin;
                    avetonRoleAccess.EntityName = entityType.Name;
                    avetonRoleAccess.EntityAction = entityAction;
                    avetonRoleAccess.IsAllowed = false;
                    avetonRoleAccess.RoleId = result.Id;
                    avetonRoleAccess.EntityOwnerId = _currentUserOwnerId;
                    await _avetonDbContext.InsertAsync(avetonRoleAccess);
                }                
            }


            if (newModel.IsDefault ?? false)
            {
                //всем пользователям её навесить
                var userDefaultRoles = new List<AvetonUsersRoles>();
                var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
                foreach (var employee in await _avetonDbContext.Employees.Where(e => e.EntityOwnerId == _currentUserOwnerId).ToListAsync())
                {
                    if (employee.CredentialsId.HasValue)
                    {
                        var userRole = new AvetonUsersRoles();
                        userRole.CreatedByUser = currentUserLogin;
                        userRole.UpdatedByUser = currentUserLogin;
                        userRole.UserId = employee.CredentialsId.Value;
                        userRole.RoleId = result.Id;
                        userRole.EntityOwnerId = _currentUserOwnerId;
                        userDefaultRoles.Add(userRole);
                    }
                }
                await _avetonDbContext.InsertRangeAsync(userDefaultRoles);
            }

            return result;
        }
        

        protected override async Task<bool> CanDeleteEntityAsync(AvetonRole entityToDelete)
        {            
            if (entityToDelete.IsSystemAdministrator 
                && (await _avetonDbContext.GetAllAsNoTrackingAsync<Employee>(e => e.Credentials.Roles.Any(e => e.Id != entityToDelete.Id && e.IsSystemAdministrator) && e.EntityOwnerId == _currentUserOwnerId)).Count == 0)
            {
                throw new LastFullAccessRoleException();
            }
            return true;
        }

        protected override IQueryable<AvetonRole> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.AvetonRoles.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filterString))
            {
                filterString = filterString.ToLower().Trim();  
                filterQuery = filterQuery.Where(e =>
                    e.Name.ToLower().Contains(filterString)
                    );  
            }
            return filterQuery;
        }

        protected override IQueryable<AvetonRole> OrderByConditionQuery(IQueryable<AvetonRole> query)
        {
            return query.OrderBy(e => e.Name).AsQueryable();
        }
    }
}
