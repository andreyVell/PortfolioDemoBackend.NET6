using FluentValidation;

namespace WebApi.DTOs.Division
{
    public class CreateDivisionRequestValidation : AbstractValidator<CreateDivisionRequest>
    {
        public CreateDivisionRequestValidation()
        {
            RuleFor(createDivisionRequest =>
                createDivisionRequest.Name).NotEmpty().MaximumLength(500);
        }
    }
}
