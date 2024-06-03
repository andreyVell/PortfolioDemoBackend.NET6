using Services.Models._BaseModels;
using Services.Models.DivisionContractor;
using Services.Models.StageManager;
using Services.Models.StageReport;

namespace Services.Models.ProjectStage
{
    public class GetProjectStageModel : ModelBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? ParentStageId { get; set; }
        public int OrderNumber { get; set; }
        public double CurrentProgress { get; set; }
        public virtual List<GetProjectStageModel>? ChildStages { get; set; }      
        /// <summary>
        /// Ответственные лица
        /// </summary>
        public virtual List<GetStageManagerModel>? StageManagers { get; set; }
        /// <summary>
        /// Отчёты
        /// </summary>
        public virtual List<GetStageReportModel>? StageReports { get; set; }
        /// <summary>
        /// Исполнители
        /// </summary>
        public virtual List<GetDivisionContractorModel>? Contractors { get; set; }
    }
}
