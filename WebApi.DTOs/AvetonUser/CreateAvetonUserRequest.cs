namespace WebApi.DTOs.AvetonUser
{
    public class CreateAvetonUserRequest
    {
        public Guid EmployeeId { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
