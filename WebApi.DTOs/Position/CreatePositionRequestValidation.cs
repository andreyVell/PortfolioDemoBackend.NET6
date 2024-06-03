using FluentValidation;

namespace WebApi.DTOs.Position
{
    public class CreatePositionRequestValidation : AbstractValidator<CreatePositionRequest>
    {
        public CreatePositionRequestValidation()
        {
            RuleFor(createDivisionRequest =>
                createDivisionRequest.Name).NotEmpty().MaximumLength(500);
        }
    }
}
