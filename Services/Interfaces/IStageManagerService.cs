using DataCore.Entities;
using Services.Models.StageManager;

namespace Services.Interfaces
{
    public interface IStageManagerService : ICrudService<StageManager, GetStageManagerModel, CreateStageManagerModel, UpdateStageManagerModel>
    {
    }
}
