using Services.Models._BaseModels;
using Services.Models.AvetonRoleAccess;

namespace Services.Models.AvetonRole
{
    public class GetAvetonRoleModel : ModelBase
    {
        public string Name { get; set; } = null!;
        public bool? IsDefault { get; set; }
        public bool IsSystemAdministrator { get; set; }

        public virtual List<GetAvetonRoleAccessModel> Accesses { get; set; }
    }
}
