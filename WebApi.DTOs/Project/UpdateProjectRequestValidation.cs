using FluentValidation;

namespace WebApi.DTOs.Project
{
    public class UpdateProjectRequestValidation : AbstractValidator<UpdateProjectRequest>
    {
        public UpdateProjectRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(createPersonRequest =>
                createPersonRequest.Name).NotEmpty().MaximumLength(1000);
            RuleFor(createPersonRequest =>
                createPersonRequest.Description).MaximumLength(5000);
        }
    }
}
