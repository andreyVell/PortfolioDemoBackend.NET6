using System.Text.Json.Serialization;

namespace DataCore.Entities
{
    public class AvetonUser
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        [JsonIgnore]
        public virtual SystemOwner Owner { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Login { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;


        public AvetonUser()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public virtual Employee? Employee { get; set; }
        public virtual ICollection<AvetonRole> Roles { get; set; } = new List<AvetonRole>();
        public virtual ICollection<AvetonUsersRoles> NavigationUserRoles { get; set; } = new List<AvetonUsersRoles>();
    }
}
