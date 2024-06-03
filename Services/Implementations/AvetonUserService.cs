using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataCore.Exceptions.AvetonUser;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.AvetonRole;
using Services.Models.AvetonUser;

namespace Services.Implementations
{
    public class AvetonUserService : IAvetonUserService
    {
        protected readonly IAvetonDbContext _avetonDbContext;
        protected readonly IMapper _mapper;
        protected readonly ICurrentUserDataService _currentUserService;
        protected readonly Guid _currentUserOwnerId;

        public AvetonUserService(
            IAvetonDbContext avetonDbContext, 
            IMapper mapper,
            ICurrentUserDataService currentUserService)
        {
            _avetonDbContext = avetonDbContext;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _currentUserOwnerId = _currentUserService.GetCurrentUserOwnerId();
        }

        public async Task<SuccessfullCreateModel> CreateAsync(CreateAvetonUserModel newModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(AvetonUser).Name, DataCore.Enums.EntityAction.Create))
            {
                throw new ActionNotAllowedException();
            }
            var dbEmployee = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<Employee>(x => x.Id == newModel.EmployeeId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbEmployee == null)
            {
                throw new EntityNotFoundException(nameof(Employee));
            }
            var existedUser = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<AvetonUser>(x => x.Login == newModel.Login && x.OwnerId == _currentUserOwnerId);
            if (existedUser != null)
            {
                throw new UserAlreadyExistsException();
            }
            var newUser = new AvetonUser();
            newUser.Login = newModel.Login;
            newUser.PasswordSalt = Guid.NewGuid().ToString();
            newUser.PasswordHash = Encryption.CreatePasswordHash(newModel.Password, newUser.PasswordSalt);
            newUser.OwnerId = _currentUserOwnerId;
            var result = await _avetonDbContext.InsertAsync(newUser);
            dbEmployee.CredentialsId = result.Id;
            await _avetonDbContext.UpdateAsync(dbEmployee);

            //userDefaultRoles
            var defaultRoles = await _avetonDbContext.GetAllAsync<AvetonRole>(e => (e.IsDefault ?? false) && e.EntityOwnerId == _currentUserOwnerId);
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            var userRoles = new List<AvetonUsersRoles>();
            foreach (var defaultRole in defaultRoles)
            {
                var userRole = new AvetonUsersRoles();
                userRole.CreatedByUser = currentUserLogin;
                userRole.UpdatedByUser = currentUserLogin;
                userRole.UserId = result.Id;
                userRole.RoleId = defaultRole.Id;
                userRole.EntityOwnerId = _currentUserOwnerId;
                userRoles.Add(userRole);
            }
            await _avetonDbContext.InsertRangeAsync(userRoles);
            return new SuccessfullCreateModel(result);
        }

        public async Task<SuccessfullUpdateModel> UpdateAsync(UpdateAvetonUserModel updateModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(AvetonUser).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbModel = await _avetonDbContext.AvetonUsers.Where(x => x.Id == updateModel.Id && x.OwnerId == _currentUserOwnerId).Include(e=>e.Employee).Include(e => e.NavigationUserRoles).AsNoTracking().FirstOrDefaultAsync();
            if (dbModel == null)
            {
                throw new EntityNotFoundException(nameof(AvetonUser));
            }
            var existedUser = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<AvetonUser>(x => x.Login == updateModel.Login && x.Id != updateModel.Id && x.OwnerId == _currentUserOwnerId);
            if (existedUser != null)
            {
                throw new UserAlreadyExistsException();
            }
            CheckLostUpdate(dbModel, updateModel);
            dbModel.CreatedOn = updateModel.CreatedOn;
            dbModel.UpdatedOn = DateTime.UtcNow;            
            dbModel.PasswordSalt = Guid.NewGuid().ToString();
            dbModel.PasswordHash = Encryption.CreatePasswordHash(updateModel.Password, dbModel.PasswordSalt);
            
            if (dbModel.Login != updateModel.Login)
            {
                dbModel.Login = updateModel.Login;
                var employee = dbModel.Employee;
                if (employee != null)
                {
                    employee.CredentialsId = null;
                    await _avetonDbContext.UpdateAsync(employee);
                }


                var userRoles = dbModel.NavigationUserRoles.ToList();
                await _avetonDbContext.DeleteRangeAsync(userRoles);
                
                await _avetonDbContext.DeleteAsync(dbModel);
                var result = await _avetonDbContext.InsertAsync(dbModel);
                                
                if (employee != null)
                {
                    employee.CredentialsId = result.Id;
                    await _avetonDbContext.UpdateAsync(employee);
                }
                await _avetonDbContext.InsertRangeAsync(userRoles);                
                return new SuccessfullUpdateModel(result);
            }
            else
            {
                var result = await _avetonDbContext.UpdateAsync(dbModel);
                return new SuccessfullUpdateModel(result);
            }
            
        }

        public async Task DeleteRoleAsync(Guid? roleId, Guid? userId)
        {
            if (roleId == null || userId == null) return;
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(AvetonUser).Name, DataCore.Enums.EntityAction.Delete))
            {
                throw new ActionNotAllowedException();
            }
            var model = await _avetonDbContext.AvetonUsersRoles.Where(e => e.RoleId == roleId && e.UserId == userId && e.EntityOwnerId == _currentUserOwnerId).FirstOrDefaultAsync();
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(AvetonUser));
            }
            var role = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<AvetonRole>(e=>e.Id == roleId.Value && e.EntityOwnerId == _currentUserOwnerId);
            if (role!.IsSystemAdministrator 
                && (await _avetonDbContext.GetAllAsNoTrackingAsync<Employee>(e => e.Credentials!.Roles.Any(e => e.IsSystemAdministrator) && e.EntityOwnerId == _currentUserOwnerId)).Count <= 1)
            {
                throw new LastFullAccessEmployeeException();
            }
            await _avetonDbContext.DeleteAsync(model);
        }

        public async Task<List<GetAvetonRoleModel>> GetAllRolesAsync(Guid? userId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(AvetonUser).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbAvetonUser = await _avetonDbContext.GetFirstOrDefaultAsync<AvetonUser>(x => x.Id == userId && x.OwnerId == _currentUserOwnerId);
            if (dbAvetonUser == null)
            {
                throw new EntityNotFoundException(nameof(AvetonUser));
            }
            return _mapper.Map<List<GetAvetonRoleModel>>(dbAvetonUser.Roles);
        }

        public async Task AddRoleToUserAsync(Guid? roleId, Guid? userId)
        {
            if (roleId == null || userId == null) return;
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(AvetonUser).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbUser = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<AvetonUser>(e=>e.Id == userId && e.OwnerId == _currentUserOwnerId);
            if (dbUser == null)
            {
                throw new EntityNotFoundException(nameof(AvetonUser));
            }
            var dbRole = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<AvetonRole>(e => e.Id == roleId && e.EntityOwnerId == _currentUserOwnerId);
            if (dbRole == null)
            {
                throw new EntityNotFoundException(nameof(AvetonRole));
            }
            var userRole = new AvetonUsersRoles();
            var currentUserLogin = (await _currentUserService.GetCurrentUserAsync()).Login;
            userRole.CreatedByUser = currentUserLogin;
            userRole.UpdatedByUser = currentUserLogin;
            userRole.UserId = dbUser.Id;
            userRole.RoleId = dbRole.Id;
            userRole.EntityOwnerId = _currentUserOwnerId;
            await _avetonDbContext.InsertAsync(userRole);
        }

        public async Task DeleteAsync(Guid? userId)
        {
            if (userId == null) return;
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(AvetonUser).Name, DataCore.Enums.EntityAction.Delete))
            {
                throw new ActionNotAllowedException();
            }
            var dbUser = await _avetonDbContext.GetFirstOrDefaultAsync<AvetonUser>(e => e.Id == userId && e.OwnerId == _currentUserOwnerId);
            if (dbUser == null) return;
            if (dbUser.NavigationUserRoles != null)
            {
                await _avetonDbContext.DeleteRangeAsync(dbUser.NavigationUserRoles);                
            }
            await _avetonDbContext.DeleteAsync(dbUser);
        }

        protected void CheckLostUpdate(AvetonUser dbModel, UpdateAvetonUserModel updateModel)
        {
            if (dbModel.UpdatedOn.Year != updateModel.UpdatedOn.Year
                || dbModel.UpdatedOn.Month != updateModel.UpdatedOn.Month
                || dbModel.UpdatedOn.Day != updateModel.UpdatedOn.Day
                || dbModel.UpdatedOn.Hour != updateModel.UpdatedOn.Hour
                || dbModel.UpdatedOn.Minute != updateModel.UpdatedOn.Minute
                || dbModel.UpdatedOn.Second != updateModel.UpdatedOn.Second
                || dbModel.UpdatedOn.Millisecond != updateModel.UpdatedOn.Millisecond)
            {
                throw new EntityLostUpdateException(nameof(updateModel));
            }
        }
    }
}
