using Services.Models._BaseModels;

namespace Services.Models.Division
{
    public class GetDivisionModel : ModelBase
    {
        public string? Name { get; set; }
        public Guid? ParentDivisionId { get; set; }
    }
}
