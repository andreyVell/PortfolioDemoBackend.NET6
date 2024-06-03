using FluentValidation;

namespace WebApi.DTOs.Employees
{
    public class CreateEmployeeRequestValidation : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeRequestValidation()
        {
            RuleFor(createEmployeeRequest =>
                createEmployeeRequest.FirstName).NotEmpty().MaximumLength(250);
            RuleFor(createEmployeeRequest =>
                createEmployeeRequest.LastName).NotEmpty().MaximumLength(250);
            RuleFor(createEmployeeRequest =>
                createEmployeeRequest.SecondName).MaximumLength(250);
            RuleFor(createEmployeeRequest =>
                createEmployeeRequest.Email).EmailAddress().Unless(x => string.IsNullOrWhiteSpace(x.Email)).MaximumLength(250);
            RuleFor(createEmployeeRequest =>
                createEmployeeRequest.MobilePhoneNumber).MaximumLength(250);
        }
    }
}
