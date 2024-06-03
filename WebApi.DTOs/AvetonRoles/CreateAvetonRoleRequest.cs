namespace WebApi.DTOs.AvetonRoles
{
    public class CreateAvetonRoleRequest
    {
        public string Name { get; set; } = null!;
        public bool? IsDefault { get; set; }
        public bool IsSystemAdministrator { get; set; }
    }
}
