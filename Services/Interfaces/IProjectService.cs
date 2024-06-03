using DataCore.Entities;
using Services.Models.Project;

namespace Services.Interfaces
{
    public interface IProjectService : ICrudService<Project, GetProjectModel, CreateProjectModel,  UpdateProjectModel>
    {
        
    }
}
