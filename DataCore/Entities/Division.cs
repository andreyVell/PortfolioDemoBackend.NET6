namespace DataCore.Entities
{
    public class Division: EntityBase
    {
        public string? Name { get; set; }
        public Guid? ParentDivisionId { get; set; }
        public virtual Division? ParentDivision { get; set; }
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public virtual ICollection<Division> ChildDivisions { get; set; } = new List<Division>();

        /// <summary>
        /// Исполняемы объекты
        /// </summary>
        public virtual ICollection<ProjectStage> ProjectStages { get; set; } = new List<ProjectStage>();
        public virtual ICollection<DivisionContractor> NavigationDivisionContractors { get; set; } = new List<DivisionContractor>();
    }
}
