using FluentValidation;

namespace WebApi.DTOs.Client
{
    public class UpdateClientRequestValidation : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientRequestValidation()
        {
            RuleFor(e => e.Id).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
