using FluentValidation;

namespace WebApi.DTOs.Client
{
    public class CreateClientRequestValidation : AbstractValidator<CreateClientRequest>
    {
        public CreateClientRequestValidation()
        {

        }
    }
}
