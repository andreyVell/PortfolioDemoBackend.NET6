using Services.Models._BaseModels;

namespace Services.Models.StageReportAttachedFile
{
    public class GetStageReportAttachedFileModel : ModelBase
    {
        public string? FileName { get; set; }
        public Guid? StageReportId { get; set; }
    }
}
