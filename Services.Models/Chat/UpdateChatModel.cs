using Services.Models._BaseModels;

namespace Services.Models.Chat
{
    public class UpdateChatModel : ModelBase
    {
        public string? Name { get; set; }       
        public AttachFileModel? NewAvatar { get; set; } 
    }
}
