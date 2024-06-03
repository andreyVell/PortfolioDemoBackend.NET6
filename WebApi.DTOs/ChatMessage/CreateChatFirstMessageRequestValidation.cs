using FluentValidation;

namespace WebApi.DTOs.ChatMessage
{
    public class CreateChatFirstMessageRequestValidation : AbstractValidator<CreateChatFirstMessageRequest>
    {
        public CreateChatFirstMessageRequestValidation()
        {
            RuleFor(e => e.Text).NotEmpty();
        }
    }
}
