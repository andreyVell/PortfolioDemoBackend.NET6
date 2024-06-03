namespace WebApi.DTOs.ProjectStage
{
    public class UpdateProjectStageRequest : DTOBase
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? ParentStageId { get; set; }
        public int OrderNumber { get; set; }
    }
}
