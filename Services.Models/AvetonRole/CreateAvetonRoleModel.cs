namespace Services.Models.AvetonRole
{
    public class CreateAvetonRoleModel
    {
        public string Name { get; set; } = null!;
        public bool? IsDefault { get; set; }
        public bool IsSystemAdministrator { get; set; }
    }
}
