namespace DataCore.Entities
{
    public class ChatMessageViewedInfo : EntityBase
    {
        public Guid MessageId { get; set; }
        public Guid ViewedById { get; set; }
        public virtual ChatMessage Message { get; set; } = null!;
        public virtual ChatMember ViewedBy { get; set; } = null!;
    }
}
