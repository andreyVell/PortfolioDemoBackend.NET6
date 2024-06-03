using Services.Models._BaseModels;

namespace Services.Models.ChatMember
{
    public class GetChatEmployeeModel : ModelBase
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SecondName { get; set; }
        public Guid? CredentialsId { get; set; }
    }
}
