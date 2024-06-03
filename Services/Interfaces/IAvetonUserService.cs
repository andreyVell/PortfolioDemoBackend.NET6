using Services.Models._BaseModels;
using Services.Models.AvetonRole;
using Services.Models.AvetonUser;

namespace Services.Interfaces
{
    public interface IAvetonUserService : IServiceRegistrator
    {        
        Task<SuccessfullCreateModel> CreateAsync(CreateAvetonUserModel newModel);
        Task<SuccessfullUpdateModel> UpdateAsync(UpdateAvetonUserModel updateModel);
        Task DeleteRoleAsync(Guid? roleId, Guid? userId);
        Task<List<GetAvetonRoleModel>> GetAllRolesAsync(Guid? userId);
        Task AddRoleToUserAsync(Guid? roleId, Guid? userId);
        Task DeleteAsync(Guid? userId);
    }
}
