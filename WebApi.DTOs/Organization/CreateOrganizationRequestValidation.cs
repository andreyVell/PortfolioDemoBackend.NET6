using FluentValidation;

namespace WebApi.DTOs.Organization
{
    public class CreateOrganizationRequestValidation : AbstractValidator<CreateOrganizationRequest>
    {
        public CreateOrganizationRequestValidation()
        {
            RuleFor(createPersonRequest =>
                createPersonRequest.Name).NotEmpty().MaximumLength(300);
            RuleFor(createPersonRequest =>
                createPersonRequest.Inn)
                .MaximumLength(20);
            RuleFor(createPersonRequest =>
                createPersonRequest.ContactEmail).EmailAddress().Unless(x => string.IsNullOrWhiteSpace(x.ContactEmail)).MaximumLength(100);
            RuleFor(createPersonRequest =>
                createPersonRequest.ContactPhone).MaximumLength(100);
            RuleFor(e => e.Login).NotEmpty().MaximumLength(50);
            RuleFor(e => e.Password).NotEmpty().MaximumLength(50);
        }
    }
}
