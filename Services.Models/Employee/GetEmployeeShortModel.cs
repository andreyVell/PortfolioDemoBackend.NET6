using Services.Models._BaseModels;
using Services.Models.Job;

namespace Services.Models.Employee
{
    public class GetEmployeeShortModel : ModelBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public string? MobilePhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public GetJobModel? LastJob { get; set; }
    }
}
