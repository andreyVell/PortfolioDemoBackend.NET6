using Services.Models._BaseModels;

namespace WebApi.DTOs.StageReportAttachedFile
{
    public class CreateStageReportAttachedFileRequest
    {
        public Guid? StageReportId { get; set; }
        public AttachFileModel? File { get; set; }
    }
}
