namespace Services.Models.AvetonUser
{
    public class UpdateAvetonUserModel
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
