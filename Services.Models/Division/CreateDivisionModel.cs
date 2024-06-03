namespace Services.Models.Division
{
    public class CreateDivisionModel
    {
        public string? Name { get; set; }
        public Guid? ParentDivisionId { get; set; }
    }
}
