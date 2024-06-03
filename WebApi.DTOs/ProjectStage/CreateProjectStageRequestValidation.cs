using FluentValidation;

namespace WebApi.DTOs.ProjectStage
{
    public class CreateProjectStageRequestValidation : AbstractValidator<CreateProjectStageRequest>
    {
        public CreateProjectStageRequestValidation()
        {            
            RuleFor(createPersonRequest =>
                createPersonRequest.Name).NotEmpty().MaximumLength(1000);
            RuleFor(createPersonRequest =>
                createPersonRequest.Description).MaximumLength(5000);
        }
    }
}
