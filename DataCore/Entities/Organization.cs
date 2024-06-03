namespace DataCore.Entities
{
    public class Organization : EntityBase
    {
        public string? Name { get; set; }
        public string? Inn { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public virtual ICollection<Client> ProjectClients { get; set; } = new List<Client>();
        public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();
    }
}
