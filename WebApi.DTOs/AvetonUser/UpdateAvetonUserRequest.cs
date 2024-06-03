namespace WebApi.DTOs.AvetonUser
{
    public class UpdateAvetonUserRequest
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
