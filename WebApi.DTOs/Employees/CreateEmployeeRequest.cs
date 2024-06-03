namespace WebApi.DTOs.Employees
{
    public class CreateEmployeeRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public string? MobilePhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
        public Guid? CredentialsId { get; set; }
    }
}
