using DataCore.Entities;
using Services.Models.StageReport;

namespace Services.Interfaces
{
    public interface IStageReportService : ICrudService<StageReport, GetStageReportModel, CreateStageReportModel, UpdateStageReportModel>
    {
    }
}
