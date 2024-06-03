using FluentValidation;

namespace WebApi.DTOs.AvetonUser
{
    public class CreateAvetonUserRequestValidation: AbstractValidator<CreateAvetonUserRequest>
    {
        public CreateAvetonUserRequestValidation()
        {
            RuleFor(e => e.EmployeeId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.Login).NotEmpty().MaximumLength(50);
            RuleFor(e => e.Password).NotEmpty().MaximumLength(50);
        }
    }
}
