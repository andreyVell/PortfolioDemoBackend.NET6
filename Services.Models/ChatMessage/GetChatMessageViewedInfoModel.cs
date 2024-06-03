using Services.Models._BaseModels;

namespace Services.Models.ChatMessage
{
    public class GetChatMessageViewedInfoModel : ModelBase
    {
        public Guid MessageId { get; set; }
        public Guid ViewedById { get; set; }       
    }
}
