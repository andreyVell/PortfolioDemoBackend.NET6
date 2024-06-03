using EntityFrameworkCore.Projectables;

namespace DataCore.Entities
{
    public class Employee : EntityBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set;}
        public string? MobilePhoneNumber { get; set;}
        public DateTime? Birthday { get; set; }
        public string? PathToAvatar { get; set; }
        public string? PathToSmallAvatar { get; set; }
        public Guid? CredentialsId { get; set; }
        public virtual AvetonUser? Credentials { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public virtual ICollection<Project> ProjectsWhereThisEmployeeIsManager { get; set; } = new List<Project>();
        public virtual ICollection<StageManager> StageManagers { get; set; } = new List<StageManager>();
        /// <summary>
        /// Отчёты
        /// </summary>
        public virtual ICollection<StageReport> StageReports { get; set; } = new List<StageReport>();
        public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();

        [Projectable]
        public virtual Job? LastJobProjectable => Jobs.OrderByDescending(j => j.StartDate).FirstOrDefault();


        public virtual Job? LastJob => Jobs.OrderByDescending(j => j.StartDate).FirstOrDefault();

        public string FirstNameAndLastName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }    
}
