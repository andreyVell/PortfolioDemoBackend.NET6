using DataCore.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Helpers
{
    public static class Encryption
    {
        private static IGlobalSettings _globalSettings;
        public static void Initialize(IGlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }
        public static string CreatePasswordHash(string password, string passwordSalt)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password,
                    Encoding.ASCII.GetBytes(passwordSalt),
                    KeyDerivationPrf.HMACSHA256,
                    5000,
                    64));
        }

        public static string CreateAvetonUserToken(AvetonUser user, double durationInHours)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(_globalSettings.ClaimTypeCurrentUserEmployeeIdentifier, user?.Employee?.Id.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Role, _globalSettings.ClaimRoleUser),
                new Claim(_globalSettings.ClaimTypeOwnerIdentifier, user?.OwnerId.ToString() ?? string.Empty),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_globalSettings.AuthenticationToken));

            var maxDurationInHours = _globalSettings.UserLoginDurationInHours;
            var expiresDate = DateTime.UtcNow.AddHours(durationInHours > maxDurationInHours ? maxDurationInHours : durationInHours).AddDays(1).Date;

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
            claims: claims,
                expires: expiresDate,
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public static string CreateClientOrganizationToken(Organization org, double durationInHours)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, org.Login),
                new Claim(ClaimTypes.NameIdentifier, org.Id.ToString()),
                new Claim(ClaimTypes.Role, _globalSettings.ClaimRoleOrganization),
                new Claim(_globalSettings.ClaimTypeOwnerIdentifier, org.EntityOwnerId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_globalSettings.AuthenticationToken));

            var expiresDate = DateTime.UtcNow.AddHours(durationInHours).AddDays(1).Date;

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
            claims: claims,
                expires: expiresDate,
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public static string CreateClientPersonToken(Person person, double durationInHours)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, person.Login),
                new Claim(ClaimTypes.NameIdentifier, person.Id.ToString()),
                new Claim(ClaimTypes.Role, _globalSettings.ClaimRolePerson),
                new Claim(_globalSettings.ClaimTypeOwnerIdentifier, person.EntityOwnerId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_globalSettings.AuthenticationToken));

            var expiresDate = DateTime.UtcNow.AddHours(durationInHours).AddDays(1).Date;

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
            claims: claims,
                expires: expiresDate,
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
