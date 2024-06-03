using DataCore.Entities;
using DataCore.Exceptions.AvetonUser;
using DataProvider;
using Services.Helpers;
using Services.Interfaces;
using Services.Models.Authentication;

namespace Services.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAvetonDbContext _avetonDbContext;
        private readonly ISubscriptionService _subscriptionService;

        public AuthenticationService(
            ISubscriptionService subscriptionService,
            IAvetonDbContext avetonDbContext)
        {
            _avetonDbContext = avetonDbContext;
            _subscriptionService = subscriptionService;
        }

        public async Task<string> LoginAsync(LoginUserModel user)
        {

            var dbUser = await _avetonDbContext.GetFirstOrDefaultAsync<AvetonUser>(e => e.Login == user.Login);
            if (dbUser == null) throw new UserLoginIncorrectDataException();
            var authHours = await _subscriptionService.GetOrganizationSubscriptionHoursAsync(dbUser.OwnerId);
            if (CanAuthenticateUser(dbUser, user) && authHours>0)
            {
                string token = Encryption.CreateAvetonUserToken(dbUser, authHours);
                return token;
            }
            else 
            {
                throw new UserLoginIncorrectDataException();
            }
        }        

        private bool CanAuthenticateUser(AvetonUser dbUser, LoginUserModel loginUser)
        {
            if (dbUser == null || loginUser == null) return false;
            if (dbUser.PasswordHash != Encryption.CreatePasswordHash(loginUser.Password, dbUser.PasswordSalt)) return false;            
            return true;
        }
    }
}
