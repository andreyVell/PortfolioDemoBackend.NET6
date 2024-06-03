using DataCore.Enums;

namespace WebApi.DTOs.ChatMember
{
    public class CreateChatMemberRequest
    {
        public Guid? ChatId { get; set; }
        public ChatMemberType Type { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? PersonClientId { get; set; }
        public Guid? OrganizationClientId { get; set; }
    }
}
