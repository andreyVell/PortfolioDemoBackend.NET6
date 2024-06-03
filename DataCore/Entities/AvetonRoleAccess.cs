using DataCore.Enums;

namespace DataCore.Entities
{
    public class AvetonRoleAccess : EntityBase
    {
        public string EntityName { get; set; } = null!;
        public EntityAction EntityAction { get; set; }
        public bool IsAllowed { get; set; }
        public Guid RoleId { get; set; }
        public virtual AvetonRole? Role { get; set; }
    }
}
