using DataCore.Enums;

namespace WebApi.DTOs.AvetonRoles
{
    public class UpdateAvetonRoleAccessRequest : DTOBase
    {
        public string EntityName { get; set; } = null!;
        public EntityAction EntityAction { get; set; }
        public bool IsAllowed { get; set; }
    }
}
