using Services.Models._BaseModels;

namespace Services.Models.StageReport
{
    public class CreateStageReportModel
    {
        public DateTime ReportDate { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public Guid? ProjectStageId { get; set; }   
        public List<AttachFileModel>? AttachedFiles { get; set; }
    }
}
