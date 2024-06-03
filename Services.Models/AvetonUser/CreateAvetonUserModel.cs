namespace Services.Models.AvetonUser
{
    public class CreateAvetonUserModel
    {
        public Guid EmployeeId { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
