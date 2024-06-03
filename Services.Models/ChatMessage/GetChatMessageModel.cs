using Services.Models._BaseModels;

namespace Services.Models.ChatMessage
{
    public class GetChatMessageModel : ModelBase
    {
        public Guid OwnerId { get; set; }
        public Guid ChatId { get; set; }
        public bool? IsSystem { get; set; }
        public string? Text { get; set; }
        public virtual List<GetChatMessageViewedInfoModel>? ViewedInfos { get; set; }
        public virtual List<GetChatMessageAttachedFileModel>? AttachedFiles { get; set; }
    }
}
