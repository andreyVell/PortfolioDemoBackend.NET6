namespace DataCore.Entities
{
    public class ChatMessageAttachedFile : AttachedFileBase
    {
        public Guid? ChatId { get; set; }
        public Guid? MessageId { get; set; }
        public virtual ChatMessage Message { get; set; } = null!;
        public virtual Chat Chat { get; set; } = null!;
    }
}
