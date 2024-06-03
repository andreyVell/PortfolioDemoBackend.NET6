namespace WebApi.DTOs.StageManager
{
    public class CreateStageManagerRequest
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}
