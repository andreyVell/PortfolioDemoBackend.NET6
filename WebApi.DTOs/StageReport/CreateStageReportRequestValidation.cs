using FluentValidation;

namespace WebApi.DTOs.StageReport
{
    public class CreateStageReportRequestValidation : AbstractValidator<CreateStageReportRequest>
    {
        public CreateStageReportRequestValidation()
        {
            RuleFor(e => e.ProjectStageId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
