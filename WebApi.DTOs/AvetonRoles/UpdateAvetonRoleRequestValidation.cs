using FluentValidation;

namespace WebApi.DTOs.AvetonRoles
{
    public class UpdateAvetonRoleRequestValidation : AbstractValidator<UpdateAvetonRoleRequest>
    {
        public UpdateAvetonRoleRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.CreatedByUser).MaximumLength(100);
            RuleFor(e => e.UpdatedByUser).MaximumLength(100);
            RuleFor(e => e.Name).NotEmpty().MaximumLength(100);
        }
    }
}
