namespace WebApi.DTOs.Organization
{
    public class CreateOrganizationRequest
    {
        public string? Name { get; set; }
        public string? Inn { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
