namespace WebApi.DTOs.AvetonRoles
{
    public class UpdateAvetonRoleRequest : DTOBase
    {
        public string Name { get; set; } = null!;
        public bool? IsDefault { get; set; }
        public bool IsSystemAdministrator { get; set; }
        public UpdateAvetonRoleAccessRequest[] Accesses { get; set; }
    }
}
