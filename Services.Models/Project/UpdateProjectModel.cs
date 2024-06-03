using Services.Models._BaseModels;

namespace Services.Models.Project
{
    public class UpdateProjectModel : ModelBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
