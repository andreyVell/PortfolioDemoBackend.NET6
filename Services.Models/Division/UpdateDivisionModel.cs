using Services.Models._BaseModels;

namespace Services.Models.Division
{
    public class UpdateDivisionModel : ModelBase
    {
        public string? Name { get; set; }
        public Guid? ParentDivisionId { get; set; }
    }
}
