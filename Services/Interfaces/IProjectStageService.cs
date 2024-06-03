using DataCore.Entities;
using Services.Models.ProjectStage;

namespace Services.Interfaces
{
    public interface IProjectStageService : ICrudService<ProjectStage, GetProjectStageModel, CreateProjectStageModel, UpdateProjectStageModel>
    {
        Task<List<GetProjectStageModel>> GetAllForProjectAsync(Guid projectId, string? filterString = "");

        Task<string> GetProjectNameAsync(Guid entityId);
    }
}
