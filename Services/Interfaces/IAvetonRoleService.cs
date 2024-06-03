using DataCore.Entities;
using Services.Models.AvetonRole;

namespace Services.Interfaces
{
    public interface IAvetonRoleService : ICrudService<AvetonRole, GetAvetonRoleModel, CreateAvetonRoleModel, UpdateAvetonRoleModel>
    {
    }
}
