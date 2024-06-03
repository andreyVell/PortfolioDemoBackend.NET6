namespace WebApi.DTOs.Division
{
    public class UpdateDivisionRequest : DTOBase
    {
        public string? Name { get; set; }
        public Guid? ParentDivisionId { get; set; }
    }
}
