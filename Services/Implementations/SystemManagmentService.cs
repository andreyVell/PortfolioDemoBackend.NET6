using DataCore.Entities;
using DataCore.Enums;
using DataProvider;
using Services.Helpers;
using Services.Interfaces;
using Services.Models.SystemManagment;
using System.Net.Http.Json;

namespace Services.Implementations
{
    public class SystemManagmentService : ISystemManagmentService
    {
        private readonly IGlobalSettings _globalSettings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAvetonDbContext _avetonDbContext;
        private readonly IFileProviderService _fileProviderService;

        public SystemManagmentService(
            IGlobalSettings globalSettings, 
            IHttpClientFactory httpClientFactory, 
            IAvetonDbContext avetonDbContext, 
            IFileProviderService fileProviderService)
        {
            _httpClientFactory = httpClientFactory;
            _globalSettings = globalSettings;
            _avetonDbContext = avetonDbContext;
            _fileProviderService = fileProviderService;
        }

        public async Task ActivateTrialAsync(Guid organizationId)
        {
            //1) запрос за инфой об орге в админку
            using (var client = _httpClientFactory.CreateClient())
            {
                var adminServerURL = _globalSettings.AdminServerURL;
                var organizationFromAdminServerModel = await client.GetFromJsonAsync<GetOrganizationFromAdminServerModel>(adminServerURL + "/Organization/" + organizationId);                
                if (organizationFromAdminServerModel == null)
                {
                    throw new Exception("Сервер не располагает информацией об организации");
                }
                //2) сделать новую запись в SystemOwners (Id = organizationId)
                await AddNewSystemOwner(organizationFromAdminServerModel);
                //3) создать админского юзера (логин и пароль на основании данных из админки)
                var adminUserId = await AddNewAvetonAdminUser(organizationFromAdminServerModel);                
                //4) создать админскую роль и навесить её админскому юзеру
                await AddNewAvetonAdminRole(organizationFromAdminServerModel, adminUserId);
                //5) создать админского сотрудника и дать ему админского юзера
                await AddNewAdminEmployee(organizationFromAdminServerModel, adminUserId);
            }
        }

        public async Task DeactivateTrialAsync(Guid organizationId)
        {
            var owner = await _avetonDbContext.GetFirstOrDefaultAsync<SystemOwner>(x => x.Id == organizationId);            
            
            if (owner != null)
            {
                var attachedFilesToDelete = owner.StageReportAttachedFiles.Where(e => !string.IsNullOrWhiteSpace(e.FilePath)).Select(x => x.FilePath!).ToList();
                attachedFilesToDelete.AddRange(owner.StageReportAttachedFiles.Where(e => !string.IsNullOrWhiteSpace(e.ImageMediumSizeFilePath)).Select(x => x.ImageMediumSizeFilePath!));
                attachedFilesToDelete.AddRange(owner.Employees.Where(e => !string.IsNullOrWhiteSpace(e.PathToAvatar)).Select(x => x.PathToAvatar!));
                attachedFilesToDelete.AddRange(owner.Employees.Where(e => !string.IsNullOrWhiteSpace(e.PathToSmallAvatar)).Select(x => x.PathToSmallAvatar!));
                attachedFilesToDelete.AddRange(owner.ChatMessageAttachedFiles.Where(e => !string.IsNullOrWhiteSpace(e.FilePath)).Select(x => x.FilePath!));
                attachedFilesToDelete.AddRange(owner.ChatMessageAttachedFiles.Where(e => !string.IsNullOrWhiteSpace(e.ImageMediumSizeFilePath)).Select(x => x.ImageMediumSizeFilePath!));
                attachedFilesToDelete.AddRange(owner.Chats.Where(e => !string.IsNullOrWhiteSpace(e.PathToAvatarBigImage)).Select(x => x.PathToAvatarBigImage!));
                attachedFilesToDelete.AddRange(owner.Chats.Where(e => !string.IsNullOrWhiteSpace(e.PathToAvatarSmallImage)).Select(x => x.PathToAvatarSmallImage!));
                await _fileProviderService.DeleteFilesAsync(attachedFilesToDelete);
                await _avetonDbContext.DeleteAsync(owner);
            }            
        }

        private async Task AddNewSystemOwner(GetOrganizationFromAdminServerModel newOrganization)
        {
            var newOwner = new SystemOwner();
            newOwner.Id = newOrganization.Id;
            newOwner.Name = newOrganization.FullName;
            await _avetonDbContext.InsertAsync(newOwner);
        }

        private async Task<Guid> AddNewAvetonAdminUser(GetOrganizationFromAdminServerModel newOrganization)
        {
            var adminUser = new AvetonUser();
            adminUser.Id = Guid.NewGuid();
            adminUser.UpdatedOn = DateTime.UtcNow;
            adminUser.CreatedOn = DateTime.UtcNow;
            adminUser.OwnerId = newOrganization.Id;
            adminUser.Login = newOrganization.AdminLogin!;
            adminUser.PasswordSalt = Guid.NewGuid().ToString();
            adminUser.PasswordHash = Encryption.CreatePasswordHash(newOrganization.AdminPassword ?? string.Empty, adminUser.PasswordSalt);  
            await _avetonDbContext.InsertAsync(adminUser);
            return adminUser.Id;
        }

        private async Task AddNewAvetonAdminRole(GetOrganizationFromAdminServerModel newOrganization, Guid adminUserId)
        {
            var adminRole = new AvetonRole();
            adminRole.Id = Guid.NewGuid();
            adminRole.CreatedByUser = newOrganization.AdminLogin!;
            adminRole.UpdatedByUser = newOrganization.AdminLogin!;
            adminRole.UpdatedOn = DateTime.UtcNow;
            adminRole.CreatedOn = DateTime.UtcNow;
            adminRole.IsDefault = false;
            adminRole.EntityOwnerId = newOrganization.Id;
            adminRole.IsSystemAdministrator = true;
            adminRole.Name = "Системный администратор";
            await _avetonDbContext.InsertAsync(adminRole);

            var usersRoles = new AvetonUsersRoles();
            usersRoles.Id = Guid.NewGuid();
            usersRoles.CreatedByUser = newOrganization.AdminLogin!;
            usersRoles.UpdatedByUser = newOrganization.AdminLogin!;
            usersRoles.UpdatedOn = DateTime.UtcNow;
            usersRoles.CreatedOn = DateTime.UtcNow;
            usersRoles.UserId = adminUserId;
            usersRoles.EntityOwnerId = newOrganization.Id;
            usersRoles.RoleId = adminRole.Id;
            await _avetonDbContext.InsertAsync(usersRoles);


            var baseType = typeof(EntityBase);
            var excludeTypes = new List<Type>() { typeof(AvetonRoleAccess), typeof(AvetonUsersRoles), typeof(SystemOwner) };
            var assembly = baseType.Assembly;

            var entityTypes = assembly.GetTypes().Where(t => t != baseType && !excludeTypes.Contains(t) && baseType.IsAssignableFrom(t)).ToList();
            entityTypes.Add(typeof(AvetonUser));

            var entityActions = new List<EntityAction>(4) { EntityAction.Create, EntityAction.Read, EntityAction.Update, EntityAction.Delete };
            foreach (var entityType in entityTypes)
            {
                foreach (var entityAction in entityActions)
                {
                    var avetonRoleAccess = new AvetonRoleAccess();
                    avetonRoleAccess.CreatedByUser = newOrganization.AdminLogin!;
                    avetonRoleAccess.UpdatedByUser = newOrganization.AdminLogin!;
                    avetonRoleAccess.EntityName = entityType.Name;
                    avetonRoleAccess.EntityAction = entityAction;
                    avetonRoleAccess.IsAllowed = true;
                    avetonRoleAccess.RoleId = adminRole.Id;
                    avetonRoleAccess.EntityOwnerId = newOrganization.Id; ;
                    await _avetonDbContext.InsertAsync(avetonRoleAccess);
                }
            }
        }


        private async Task AddNewAdminEmployee(GetOrganizationFromAdminServerModel newOrganization, Guid adminUserId)
        {
            var employee = new Employee();
            employee.Id = Guid.NewGuid();
            employee.EntityOwnerId = newOrganization.Id;
            employee.CreatedByUser = newOrganization.AdminLogin!;
            employee.UpdatedByUser = newOrganization.AdminLogin!;
            employee.UpdatedOn = DateTime.UtcNow;
            employee.CreatedOn = DateTime.UtcNow;
            employee.CredentialsId = adminUserId;
            employee.LastName = "Системный";
            employee.FirstName = "Администратор";
            employee.SecondName = string.Empty;
            await _avetonDbContext.InsertAsync(employee);
        }
    }
}
