namespace WebApi.DTOs.Project
{
    public class UpdateProjectRequest : DTOBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ManagerId { get; set; }
    }
}
