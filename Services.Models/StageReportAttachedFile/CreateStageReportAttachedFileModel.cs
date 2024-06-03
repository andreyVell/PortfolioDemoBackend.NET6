using Services.Models._BaseModels;

namespace Services.Models.StageReportAttachedFile
{
    public class CreateStageReportAttachedFileModel
    {
        public Guid? StageReportId { get; set; }
        public AttachFileModel? File { get; set; }
    }
}
