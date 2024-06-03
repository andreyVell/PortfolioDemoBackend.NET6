namespace WebApi.DTOs.Project
{
    public class CreateProjectRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
