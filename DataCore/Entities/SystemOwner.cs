namespace DataCore.Entities
{
    public class SystemOwner
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<AvetonRole> Roles { get; set; } = new List<AvetonRole>();
        public virtual ICollection<AvetonRoleAccess> Accesses { get; set; } = new List<AvetonRoleAccess>();
        public virtual ICollection<AvetonUser> Users { get; set; } = new List<AvetonUser>();
        public virtual ICollection<AvetonUsersRoles> UsersRoles { get; set; } = new List<AvetonUsersRoles>();
        public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
        public virtual ICollection<Division> Divisions { get; set; } = new List<Division>();
        public virtual ICollection<DivisionContractor> DivisionContractors { get; set; } = new List<DivisionContractor>();
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public virtual ICollection<Organization> Organizations { get; set; } = new List<Organization>();
        public virtual ICollection<Person> Persons { get; set; } = new List<Person>();
        public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<ProjectStage> ProjectStages { get; set; } = new List<ProjectStage>();
        public virtual ICollection<StageManager> StageManagers { get; set; } = new List<StageManager>();
        public virtual ICollection<StageReport> StageReports { get; set; } = new List<StageReport>();
        public virtual ICollection<StageReportAttachedFile> StageReportAttachedFiles { get; set; } = new List<StageReportAttachedFile>();
        public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();
        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();
        public virtual ICollection<ChatMessageAttachedFile> ChatMessageAttachedFiles { get; set; } = new List<ChatMessageAttachedFile>();
        public virtual ICollection<ChatMessageViewedInfo> ChatMessageViewedInfos { get; set; } = new List<ChatMessageViewedInfo>();
    }
}
