using DataCore.Entities;
using Services.Models.Position;

namespace Services.Interfaces
{
    public interface IPositionService : ICrudService<Position, GetPositionModel, CreatePositionModel, UpdatePositionModel>
    {
    }
}
