using FluentValidation;

namespace WebApi.DTOs.AvetonRoles
{
    public class UpdateAvetonRoleAccessRequestValidation : AbstractValidator<UpdateAvetonRoleAccessRequest>
    {
        public UpdateAvetonRoleAccessRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.CreatedByUser).MaximumLength(100);
            RuleFor(e => e.UpdatedByUser).MaximumLength(100);
            RuleFor(e => e.EntityName).NotEmpty().MaximumLength(100);
        }
    }
}
