namespace DataCore.Entities
{
    public class Person : EntityBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
        public virtual ICollection<Client> ProjectClients { get; set; } = new List<Client>();
        public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();
        public string FirstNameAndLastName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
