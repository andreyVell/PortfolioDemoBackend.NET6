using FluentValidation;

namespace WebApi.DTOs.DivisionContractor
{
    public class CreateDivisionContractorRequestValidation : AbstractValidator<CreateDivisionContractorRequest>
    {
        public CreateDivisionContractorRequestValidation()
        {
            RuleFor(e => e.ProjectStageId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.DivisionId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
