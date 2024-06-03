using Services.Models._BaseModels;

namespace Services.Models.Person
{
    public class GetPersonModel : ModelBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string Login { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
