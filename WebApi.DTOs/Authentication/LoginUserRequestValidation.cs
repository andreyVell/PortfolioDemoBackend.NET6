using FluentValidation;

namespace WebApi.DTOs.Authentication
{
    public class LoginUserRequestValidation : AbstractValidator<LoginUserRequest>
    {
        public LoginUserRequestValidation()
        {
            RuleFor(loginUserRequest =>
                loginUserRequest.Login).NotEmpty().MaximumLength(100);
            RuleFor(loginUserRequest =>
                loginUserRequest.Password).NotEmpty().MaximumLength(100);
        }
    }
}
