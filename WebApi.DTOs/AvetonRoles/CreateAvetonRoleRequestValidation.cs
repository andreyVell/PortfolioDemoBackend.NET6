using FluentValidation;

namespace WebApi.DTOs.AvetonRoles
{
    public class CreateAvetonRoleRequestValidation : AbstractValidator<CreateAvetonRoleRequest>
    {
        public CreateAvetonRoleRequestValidation()
        {            
            RuleFor(e => e.Name).NotEmpty().MaximumLength(100);
        }
    }
}
