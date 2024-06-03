namespace Services.Models.SystemManagment
{
    public class GetOrganizationFromAdminServerModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? FullName { get; set; }
        public string? ShortName { get; set; }
        public string? Inn { get; set; }
        public string? Notes { get; set; }
        public bool IsTrialActivated { get; set; }
        public string? AdminLogin { get; set; }
        public string? AdminPassword { get; set; }
    }
}
