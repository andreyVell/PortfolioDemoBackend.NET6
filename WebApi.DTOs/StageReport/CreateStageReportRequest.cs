using Services.Models._BaseModels;

namespace WebApi.DTOs.StageReport
{
    public class CreateStageReportRequest
    {
        public DateTime ReportDate { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public Guid? ProjectStageId { get; set; }
        public List<AttachFileModel>? AttachedFiles { get; set; }
    }
}
