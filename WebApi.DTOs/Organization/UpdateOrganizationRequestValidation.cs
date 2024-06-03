using FluentValidation;

namespace WebApi.DTOs.Organization
{
    public class UpdateOrganizationRequestValidation : AbstractValidator<UpdateOrganizationRequest>
    {
        public UpdateOrganizationRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
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
