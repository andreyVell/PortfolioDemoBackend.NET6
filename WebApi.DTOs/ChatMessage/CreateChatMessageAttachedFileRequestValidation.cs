using FluentValidation;

namespace WebApi.DTOs.ChatMessage
{
    public class CreateChatMessageAttachedFileRequestValidation : AbstractValidator<CreateChatMessageAttachedFileRequest>
    {
        public CreateChatMessageAttachedFileRequestValidation()
        {
            RuleFor(af => af.FileContent).NotNull();
        }
    }
}
