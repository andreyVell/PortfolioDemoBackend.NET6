using FluentValidation;

namespace WebApi.DTOs.Job
{
    public class UpdateJobRequestValidation : AbstractValidator<UpdateJobRequest>
    {
        public UpdateJobRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.CreatedByUser).MaximumLength(100);
            RuleFor(e => e.UpdatedByUser).MaximumLength(100);
            RuleFor(e => e.PositionId).NotEqual(Guid.Empty);
            RuleFor(e => e.DivisionId).NotEqual(Guid.Empty);
            RuleFor(e => e.EmployeeId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
