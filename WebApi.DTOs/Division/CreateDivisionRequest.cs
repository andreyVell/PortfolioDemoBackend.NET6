namespace WebApi.DTOs.Division
{
    public class CreateDivisionRequest
    {
        public string? Name { get; set; }
        public Guid? ParentDivisionId { get; set; }
    }
}
