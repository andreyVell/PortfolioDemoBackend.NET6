using Services.Models._BaseModels;
using Services.Models.Division;

namespace Services.Models.DivisionContractor
{
    public class GetDivisionContractorModel : ModelBase
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? DivisionId { get; set; }
        public virtual GetDivisionModel? Division { get; set; }
    }
}
