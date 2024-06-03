using Services.Models._BaseModels;

namespace Services.Models.DivisionContractor
{
    public class UpdateDivisionContractorModel : ModelBase
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? DivisionId { get; set; }
    }
}
