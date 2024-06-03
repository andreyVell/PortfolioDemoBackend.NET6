using FluentValidation;

namespace WebApi.DTOs.Person
{
    public class CreatePersonRequestValidation : AbstractValidator<CreatePersonRequest>
    {
        public CreatePersonRequestValidation()
        {
            RuleFor(createPersonRequest =>
                createPersonRequest.FirstName).NotEmpty().MaximumLength(250);
            RuleFor(createPersonRequest =>
                createPersonRequest.LastName).NotEmpty().MaximumLength(250);
            RuleFor(createPersonRequest =>
                createPersonRequest.SecondName).MaximumLength(250);
            RuleFor(createPersonRequest =>
                createPersonRequest.ContactEmail).EmailAddress().Unless(x => string.IsNullOrWhiteSpace(x.ContactEmail)).MaximumLength(100);
            RuleFor(createPersonRequest =>
                createPersonRequest.ContactPhone).MaximumLength(100);
            RuleFor(e => e.Login).NotEmpty().MaximumLength(50);
            RuleFor(e => e.Password).NotEmpty().MaximumLength(50);
        }
    }
}
