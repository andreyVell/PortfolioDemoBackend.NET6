using FluentValidation;

namespace WebApi.DTOs.Employees
{
    public class UpdateEmployeeRequestValidation : AbstractValidator<UpdateEmployeeRequest>
    {
        public UpdateEmployeeRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.CreatedByUser).MaximumLength(100);
            RuleFor(e => e.UpdatedByUser).MaximumLength(100);
            RuleFor(e => e.FirstName).MaximumLength(250);
            RuleFor(e => e.LastName).MaximumLength(250);
            RuleFor(e => e.SecondName).MaximumLength(250);
            RuleFor(e => e.Email).MaximumLength(250);
            RuleFor(e => e.MobilePhoneNumber).MaximumLength(250);
        }
    }
}
