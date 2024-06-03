namespace WebApi.DTOs.StageReport
{
    public class UpdateStageReportRequest : DTOBase
    {
        public DateTime ReportDate { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public Guid? ProjectStageId { get; set; }
        public Guid? StageManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
