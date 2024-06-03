namespace Services.Models.Job
{
    public class CreateJobModel
    {
        public DateTime? StartDate { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? DivisionId { get; set; }
    }
}
