using DataCore.Enums;

namespace Services.Models.ChatMember
{
    public class CreateChatMemberForNewChatModel
    {
        public ChatMemberType Type { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? PersonClientId { get; set; }
        public Guid? OrganizationClientId { get; set; }
    }
}
