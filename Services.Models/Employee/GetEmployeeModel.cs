using Services.Models._BaseModels;
using Services.Models.AvetonRole;
using Services.Models.AvetonUser;
using Services.Models.Job;

namespace Services.Models.Employee
{
    public class GetEmployeeModel : ModelBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public string? MobilePhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public Guid? CredentialsId { get; set; }
        public GetAvetonUserModel? Credentials { get; set; }
        public GetJobModel? LastJob { get; set; }
        public List<GetAvetonRoleModel> Roles { get; set; }
        public AttachFileModel? EmployeeAvatar { get; set; }
        public AttachFileModel? EmployeeSmallAvatar { get; set; }
    }
}
