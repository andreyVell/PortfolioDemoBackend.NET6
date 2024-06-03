using FluentValidation;

namespace WebApi.DTOs.Division
{
    public class UpdateDivisionRequestValidation : AbstractValidator<UpdateDivisionRequest>
    {
        public UpdateDivisionRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.CreatedByUser).MaximumLength(100);
            RuleFor(e => e.UpdatedByUser).MaximumLength(100);
            RuleFor(e => e.Name).NotEmpty().MaximumLength(500);
        }
    }
}
