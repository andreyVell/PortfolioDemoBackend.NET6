using EntityFrameworkCore.Projectables;

namespace DataCore.Entities
{
    public class Chat : EntityBase
    {
        public string? Name { get; set; }
        public string? PathToAvatarSmallImage { get; set; }
        public string? PathToAvatarBigImage { get; set; }
        public bool? IsGroupChat { get; set; }

        [Projectable]
        public virtual ChatMessage? LastMessageProjectable => Messages.OrderByDescending(j => j.CreatedOn).FirstOrDefault();
        public virtual ICollection<ChatMember> ChatMembers { get; set; } = new List<ChatMember>();
        public virtual ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
        public virtual ICollection<ChatMessageAttachedFile> AttachedFiles { get; set; } = new List<ChatMessageAttachedFile>();
    }
}
