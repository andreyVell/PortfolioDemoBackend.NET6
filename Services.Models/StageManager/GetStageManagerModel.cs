using Services.Models._BaseModels;
using Services.Models.Employee;

namespace Services.Models.StageManager
{
    public class GetStageManagerModel : ModelBase
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? EmployeeId { get; set; }        
        public virtual GetEmployeeShortModel? Employee { get; set; }        
    }
}
