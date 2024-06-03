using Services.Models._BaseModels;

namespace Services.Models.StageManager
{
    public class UpdateStageManagerModel : ModelBase
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
