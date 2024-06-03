using Services.Models._BaseModels;
using Services.Models.StageManager;
using Services.Models.StageReportAttachedFile;

namespace Services.Models.StageReport
{
    public class GetStageReportModel : ModelBase
    {
        public DateTime ReportDate { get; set; }
        public string? Name { get; set; }
        public string? Content { get; set; }
        public Guid? ProjectStageId { get; set; }
        public Guid? StageManagerId { get; set; }
        public Guid? EmployeeId { get; set; }
        public virtual GetStageManagerModel? StageManager { get; set; }
        public virtual List<GetStageReportAttachedFileModel>? AttachedFiles { get; set; }
    }
}
