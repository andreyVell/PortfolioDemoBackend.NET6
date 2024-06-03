using FluentValidation;

namespace WebApi.DTOs.StageReport
{
    public class UpdateStageReportRequestValidation : AbstractValidator<UpdateStageReportRequest>
    {
        public UpdateStageReportRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.Name).NotEmpty().MaximumLength(500);
            RuleFor(e => e.Content).MaximumLength(2000);
            RuleFor(e => e.ProjectStageId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.StageManagerId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.EmployeeId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
