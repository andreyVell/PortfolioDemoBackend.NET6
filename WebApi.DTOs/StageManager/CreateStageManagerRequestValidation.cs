using FluentValidation;

namespace WebApi.DTOs.StageManager
{
    public class CreateStageManagerRequestValidation : AbstractValidator<CreateStageManagerRequest>
    {
        public CreateStageManagerRequestValidation()
        {
            RuleFor(e => e.ProjectStageId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.EmployeeId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
