namespace DataCore.Entities
{
    public class AvetonUsersRoles : EntityBase
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public virtual AvetonUser? User { get; set; }
        public virtual AvetonRole? Role { get; set; }
    }
}
