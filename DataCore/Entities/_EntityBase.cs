using System.Text.Json.Serialization;

namespace DataCore.Entities
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }
        public Guid EntityOwnerId { get; set; }
        [JsonIgnore]
        public virtual SystemOwner? EntityOwner { get; set; }
        public string CreatedByUser  { get; set; }
        public string UpdatedByUser { get; set;}
        public DateTime CreatedOn { get; set;}
        public DateTime UpdatedOn { get; set;}

        public EntityBase()
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}