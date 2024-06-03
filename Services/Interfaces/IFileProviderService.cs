using Services.Models._BaseModels;

namespace Services.Interfaces
{
    public interface IFileProviderService : IServiceRegistrator
    {
        Task<string> SaveFileAsync(AttachFileModel attachedFileModel);
        Task<string> CompressAndSaveImageAsync(AttachFileModel attachedFileModel, int newWidth, int newHeight);
        Task<AttachFileModel?> GetFileDataUrlAsync(string? fileKey);
        Task DeleteAsync(string? fileKey);
        Task DeleteFilesAsync(IEnumerable<string> filesKeys);
    }
}
