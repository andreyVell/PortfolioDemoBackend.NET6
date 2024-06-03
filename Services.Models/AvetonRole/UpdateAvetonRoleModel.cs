using Services.Models._BaseModels;
using Services.Models.AvetonRoleAccess;

namespace Services.Models.AvetonRole
{
    public class UpdateAvetonRoleModel : ModelBase
    {
        public string Name { get; set; } = null!;
        public bool? IsDefault { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public UpdateAvetonRoleAccessModel[] Accesses { get; set; }
    }
}
