using DataCore.Enums;

namespace WebApi.DTOs.Authentication
{
    public class LoginClientRequest
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public ClientType ClientType { get; set; }
    }
}
