namespace Services.Models.Project
{
    public class CreateProjectModel
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
