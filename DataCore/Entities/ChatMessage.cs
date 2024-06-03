namespace DataCore.Entities
{
    public class ChatMessage : EntityBase
    {
        public Guid? OwnerId { get; set; }
        public Guid ChatId { get; set; }
        public bool? IsSystem { get; set; }
        public string? Text { get; set; }
        public virtual ChatMember? Owner { get; set; } = null!;
        public virtual Chat Chat { get; set; } = null!;
        public virtual ICollection<ChatMessageAttachedFile> AttachedFiles { get; set; } = new List<ChatMessageAttachedFile>();
        public virtual ICollection<ChatMessageViewedInfo> ViewedInfos { get; set; } = new List<ChatMessageViewedInfo>();
    }
}
