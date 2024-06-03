using Services.Models._BaseModels;

namespace Services.Models.Job
{
    public class GetJobModel : ModelBase
    {
        public DateTime? StartDate { get; set; }
        public Guid? PositionId { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? DivisionId { get; set; }
        public string? PositionName { get; set; }
        public string? DivisionName { get; set; }
    }
}
