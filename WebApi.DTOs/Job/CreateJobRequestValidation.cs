using FluentValidation;

namespace WebApi.DTOs.Job
{
    public class CreateJobRequestValidation : AbstractValidator<CreateJobRequest>
    {
        public CreateJobRequestValidation()
        {
            RuleFor(e => e.PositionId).NotEqual(Guid.Empty);
            RuleFor(e => e.DivisionId).NotEqual(Guid.Empty);
            RuleFor(e => e.EmployeeId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
