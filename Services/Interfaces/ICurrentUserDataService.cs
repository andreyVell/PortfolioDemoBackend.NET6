using DataCore.Enums;
using Services.Models.AvetonRoleAccess;
using Services.Models.AvetonUser;

namespace Services.Interfaces
{
    public interface ICurrentUserDataService : IServiceRegistrator
    {
        Guid GetCurrentUserId();
        Guid GetCurrentUserOwnerId();
        Task<GetAvetonUserModel> GetCurrentUserAsync();
        Task<bool> IsCurrentUserHasAccessToEntityAction(string? entityName, EntityAction action);
        Task<List<GetAvetonRoleAccessModel>> GetAccessesForCurrentUserAsync(params string[] entityNames);
        Task<Guid?> GetEmployeeIdForCurrentUserAsync();  
    }
}
