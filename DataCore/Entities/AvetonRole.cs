namespace DataCore.Entities
{
    public class AvetonRole : EntityBase
    {
        public string Name { get; set; } = null!;
        public bool? IsDefault { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public virtual ICollection<AvetonRoleAccess> Accesses { get; set; } = new List<AvetonRoleAccess>();
        public virtual ICollection<AvetonUser> Users { get; set; } = new List<AvetonUser>();
        public virtual ICollection<AvetonUsersRoles> NavigationUserRoles { get; set; } = new List<AvetonUsersRoles>();
    }
}
