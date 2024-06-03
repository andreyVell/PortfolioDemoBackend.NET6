namespace DataCore.Entities
{
    public class Job : EntityBase
    {
        public DateTime? StartDate { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? DivisionId { get; set; }

        public virtual Position? Position { get; set; }
        public virtual Employee? Employee { get; set; }
        public virtual Division? Division { get; set; }
    }
}
