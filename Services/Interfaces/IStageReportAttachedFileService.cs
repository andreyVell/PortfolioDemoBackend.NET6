using Services.Models._BaseModels;
using Services.Models.StageReportAttachedFile;

namespace Services.Interfaces
{
    public interface IStageReportAttachedFileService : IServiceRegistrator
    {
        Task DeleteAsync(Guid entityId);

        Task<AttachFileModel> GetFileContentAsync(Guid entityId, bool isImageMedium = false);

        Task<SuccessfullCreateModel> CreateAsync(CreateStageReportAttachedFileModel model);
    }
}
