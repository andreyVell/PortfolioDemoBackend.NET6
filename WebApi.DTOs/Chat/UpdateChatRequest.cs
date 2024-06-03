using Services.Models._BaseModels;

namespace WebApi.DTOs.Chat
{
    public class UpdateChatRequest : DTOBase
    {
        public string? Name { get; set; }
        public AttachFileModel? NewAvatar { get; set; }
    }
}
