using DataCore.Enums;

namespace Services.Models.Authentication
{
    public class LoginClientModel
    {
        public string? Login { get; set; }
        public string? Password { get; set; }
        public ClientType ClientType { get; set; }
    }
}
