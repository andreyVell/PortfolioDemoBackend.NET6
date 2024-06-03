using DataCore.Enums;
using Services.Models._BaseModels;

namespace Services.Models.AvetonRoleAccess
{
    public class GetAvetonRoleAccessModel : ModelBase
    {
        public string EntityName { get; set; } = null!;
        public EntityAction EntityAction { get; set; }
        public bool IsAllowed { get; set; }
    }
}
