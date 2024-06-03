using FluentValidation;

namespace WebApi.DTOs.Project
{
    public class CreateProjectRequestValidation : AbstractValidator<CreateProjectRequest>
    {
        public CreateProjectRequestValidation()
        {
            RuleFor(createPersonRequest =>
                createPersonRequest.Name).NotEmpty().MaximumLength(1000);
            RuleFor(createPersonRequest =>
                createPersonRequest.Description).MaximumLength(5000);
        }
    }
}
