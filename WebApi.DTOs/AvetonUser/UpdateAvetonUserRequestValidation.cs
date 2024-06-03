using FluentValidation;

namespace WebApi.DTOs.AvetonUser
{
    public class UpdateAvetonUserRequestValidation : AbstractValidator<UpdateAvetonUserRequest>
    {
        public UpdateAvetonUserRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.Login).NotEmpty().MaximumLength(50);
            RuleFor(e => e.Password).NotEmpty().MaximumLength(50);
        }
    }
}
