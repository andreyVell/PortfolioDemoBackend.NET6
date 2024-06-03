using FluentValidation;

namespace WebApi.DTOs.ProjectStage
{
    public class UpdateProjectStageRequestValidation : AbstractValidator<UpdateProjectStageRequest>
    {
        public UpdateProjectStageRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(createPersonRequest =>
                createPersonRequest.Name).NotEmpty().MaximumLength(1000);
            RuleFor(createPersonRequest =>
                createPersonRequest.Description).MaximumLength(5000);
        }
    }
}
