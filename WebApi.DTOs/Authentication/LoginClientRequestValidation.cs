using FluentValidation;

namespace WebApi.DTOs.Authentication
{
    public class LoginClientRequestValidation : AbstractValidator<LoginClientRequest>
    {
        public LoginClientRequestValidation()
        {
            RuleFor(loginClientRequest =>
                loginClientRequest.Login).NotEmpty().MaximumLength(100);
            RuleFor(loginClientRequest =>
                loginClientRequest.Password).NotEmpty().MaximumLength(100);
            RuleFor(loginClientRequest =>
                loginClientRequest.ClientType).IsInEnum();
        }
    }
}
