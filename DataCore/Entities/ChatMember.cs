using DataCore.Enums;

namespace DataCore.Entities
{
    public class ChatMember : EntityBase
    {

        //при удалении Employee, PersonClient или OrganizationClient поля с их ID становятся null, возможно их стоит удалять, подумать над этим.
        public Guid ChatId { get; set; }
        public ChatMemberType Type { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? PersonClientId { get; set; }
        public Guid? OrganizationClientId { get; set; }

        public virtual Chat Chat { get; set; } = null!;
        public virtual Employee? Employee { get; set; }
        public virtual Person? PersonClient { get; set; }
        public virtual Organization? OrganizationClient { get; set; }
        /// <summary>
        /// Отправленные сообщения
        /// </summary>
        public virtual ICollection<ChatMessage> OutcomingMessages { get; set; } = new List<ChatMessage>();
        /// <summary>
        /// Входящие просмотренные сообщения
        /// </summary>
        public virtual ICollection<ChatMessageViewedInfo> ViewedMessages { get; set; } = new List<ChatMessageViewedInfo>();
    }
}
