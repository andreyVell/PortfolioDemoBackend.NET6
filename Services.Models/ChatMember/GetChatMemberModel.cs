using DataCore.Enums;
using Services.Models._BaseModels;

namespace Services.Models.ChatMember
{
    public class GetChatMemberModel : ModelBase
    {
        public Guid ChatId { get; set; }
        public ChatMemberType Type { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? PersonClientId { get; set; }
        public Guid? OrganizationClientId { get; set; }

        public virtual GetChatEmployeeModel? Employee { get; set; }
        public virtual GetChatPersonModel? PersonClient { get; set; }
        public virtual GetChatOrganizationModel? OrganizationClient { get; set; }
    }
}
