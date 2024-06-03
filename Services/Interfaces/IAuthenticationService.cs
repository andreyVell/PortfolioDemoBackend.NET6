using Services.Models.Authentication;

namespace Services.Interfaces
{
    public interface IAuthenticationService : IServiceRegistrator
    {
        Task<string> LoginAsync(LoginUserModel user);
    }
}
