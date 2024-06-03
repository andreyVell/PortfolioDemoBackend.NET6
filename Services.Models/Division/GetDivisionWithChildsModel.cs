using Services.Models._BaseModels;

namespace Services.Models.Division
{
    public class GetDivisionWithChildsModel : ModelBase
    {
        public string? Name { get; set; }
        public Guid? ParentDivisionId { get; set; }
        public List<GetDivisionWithChildsModel> ChildDivisions { get; set; } = new List<GetDivisionWithChildsModel>();
    }
}
