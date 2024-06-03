using AutoMapper;
using DataCore.Entities;
using DataCore.Exceptions;
using DataProvider;
using Microsoft.EntityFrameworkCore;
using Services.Helpers;
using Services.Interfaces;
using Services.Models._BaseModels;
using Services.Models.Employee;

namespace Services.Implementations
{
    public class EmployeeService
        : CrudService<Employee, GetEmployeeModel, CreateEmployeeModel, UpdateEmployeeModel>, IEmployeeService
    {
        protected readonly IFileProviderService _fileProviderService;
        protected readonly IAvetonUserService _avetonUserService;
        protected readonly IJobService _jobService;
        protected readonly ISubscriptionService _subscriptionService;
        public EmployeeService(
            IAvetonDbContext avetonDbContext, 
            IMapper mapper, 
            ICurrentUserDataService currentUserService,
            IAvetonUserService avetonUserService,
            IJobService jobService,
            IFileProviderService fileProviderService,
            ISubscriptionService subscriptionService) 
            : base(avetonDbContext, mapper, currentUserService)
        {
            _fileProviderService = fileProviderService;
            _avetonUserService = avetonUserService;
            _jobService = jobService;
            _subscriptionService = subscriptionService;
        }
        public override async Task<SuccessfullCreateModel> CreateAsync(CreateEmployeeModel newModel)
        {
            var employeesCount = await _avetonDbContext.Employees.Where(e => e.EntityOwnerId == _currentUserOwnerId).CountAsync();
            var subscriptionLimit = await _subscriptionService.GetOrganizationMaxEmployeesAsync(_currentUserOwnerId);
            if (employeesCount >= subscriptionLimit)
            {
                throw new SubscriptionLimitException();
            }
            return await base.CreateAsync(newModel);
        }

        public override async Task<SuccessfullUpdateModel> UpdateAsync(UpdateEmployeeModel updateModel)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Employee).Name, DataCore.Enums.EntityAction.Update))
            {
                throw new ActionNotAllowedException();
            }
            var dbModel = await _avetonDbContext.GetFirstOrDefaultAsNoTrackingAsync<Employee>(x => x.Id == updateModel.Id && x.EntityOwnerId == _currentUserOwnerId);
            if (dbModel == null)
            {
                throw new EntityNotFoundException(nameof(Employee));
            }
            CheckLostUpdate(dbModel, updateModel);
            var dbModelOwnerId = dbModel.EntityOwnerId;
            var pathToAvatar = dbModel.PathToAvatar;
            var pathToSmallAvatar = dbModel.PathToSmallAvatar;
            dbModel = _mapper.Map<Employee>(updateModel);
            dbModel.UpdatedByUser = (await _currentUserService.GetCurrentUserAsync()).Login;
            dbModel.UpdatedOn = DateTime.UtcNow;
            dbModel.EntityOwnerId = dbModelOwnerId;
            dbModel.PathToAvatar = pathToAvatar;
            dbModel.PathToSmallAvatar = pathToSmallAvatar;

            if (updateModel.EmployeeAvatar != null)
            {
                //check if employeeAvatar is image
                if (!ImageFormatter.IsBase64StringIsImage(updateModel.EmployeeAvatar.FileContent))
                {
                    throw new FileIsNotImageException();
                }
                if (!string.IsNullOrWhiteSpace(dbModel.PathToAvatar))
                {
                    await _fileProviderService.DeleteAsync(dbModel.PathToAvatar);
                }
                if (!string.IsNullOrWhiteSpace(dbModel.PathToSmallAvatar))
                {
                    await _fileProviderService.DeleteAsync(dbModel.PathToSmallAvatar);
                }
                dbModel.PathToAvatar = await _fileProviderService.CompressAndSaveImageAsync(updateModel.EmployeeAvatar, 1000, 1500);
                dbModel.PathToSmallAvatar = await _fileProviderService.CompressAndSaveImageAsync(updateModel.EmployeeAvatar, 60, 60);
            }

            var result = await _avetonDbContext.UpdateAsync(dbModel);
            return new SuccessfullUpdateModel(result);
        }

        public override async Task<GetEmployeeModel> GetAsync(Guid entityId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Employee).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var model = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(x => x.Id == entityId && x.EntityOwnerId == _currentUserOwnerId);
            if (model == null)
            {
                throw new EntityNotFoundException(nameof(Employee));
            }
            var mappedResult = _mapper.Map<GetEmployeeModel>(model);
            if (!string.IsNullOrWhiteSpace(model.PathToAvatar))
            {
                mappedResult.EmployeeAvatar = await _fileProviderService.GetFileDataUrlAsync(model.PathToAvatar);
            }
            return mappedResult;
        }

        public async Task<AttachFileModel?> GetEmployeeSmallAvatarAsync(Guid employeeId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Employee).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbEmployee = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(x => x.Id == employeeId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbEmployee == null)
            {
                throw new EntityNotFoundException(nameof(Employee));
            }            
            var smallAvatar = await _fileProviderService.GetFileDataUrlAsync(dbEmployee.PathToSmallAvatar);
            return smallAvatar;
        }

        public async Task<AttachFileModel?> GetEmployeeBigAvatarAsync(Guid employeeId)
        {
            if (!await _currentUserService.IsCurrentUserHasAccessToEntityAction(typeof(Employee).Name, DataCore.Enums.EntityAction.Read))
            {
                throw new ActionNotAllowedException();
            }
            var dbEmployee = await _avetonDbContext.GetFirstOrDefaultAsync<Employee>(x => x.Id == employeeId && x.EntityOwnerId == _currentUserOwnerId);
            if (dbEmployee == null)
            {
                throw new EntityNotFoundException(nameof(Employee));
            }
            return await _fileProviderService.GetFileDataUrlAsync(dbEmployee.PathToAvatar);
        }

        protected override async Task<bool> CanDeleteEntityAsync(Employee entityToDelete)
        {
            var dbSysAdmins = await _avetonDbContext.GetAllAsNoTrackingAsync<Employee>(e => e.Credentials!.Roles.Any(e => e.IsSystemAdministrator && e.EntityOwnerId == _currentUserOwnerId));
            if (dbSysAdmins != null && dbSysAdmins.Count <= 1 && dbSysAdmins.FirstOrDefault()?.Id == entityToDelete.Id)
            {
                throw new LastFullAccessEmployeeException();
            }
            foreach (var job in entityToDelete!.Jobs.ToList())
            {
                await _jobService.DeleteAsync(job.Id);
            }            
            await _avetonUserService.DeleteAsync(entityToDelete.CredentialsId);
            await _fileProviderService.DeleteAsync(entityToDelete.PathToAvatar);
            await _fileProviderService.DeleteAsync(entityToDelete.PathToSmallAvatar);
            return true;
        }

        protected override IQueryable<Employee> GetFilterQuery(string filterString)
        {
            var filterQuery = _avetonDbContext.Employees.AsQueryable();
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
                        || e.Credentials!.Roles.Any(e=>e.Name.ToLower().Contains(filterString)) 
                        );
                }
                    
                
            }
            return filterQuery;
        }

        protected override IQueryable<Employee> OrderByConditionQuery(IQueryable<Employee> query)
        {
            return query.OrderBy(e => e.LastName + e.FirstName + e.SecondName).AsQueryable();
        }        
    }
}
