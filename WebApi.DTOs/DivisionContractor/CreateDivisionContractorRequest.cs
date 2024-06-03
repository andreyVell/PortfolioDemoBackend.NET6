namespace WebApi.DTOs.DivisionContractor
{
    public class CreateDivisionContractorRequest
    {
        public Guid? ProjectStageId { get; set; }
        public Guid? DivisionId { get; set; }
    }
}
