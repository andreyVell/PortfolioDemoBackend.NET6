using FluentValidation;

namespace WebApi.DTOs.Chat
{
    public class CreateChatRequestValidation : AbstractValidator<CreateChatRequest>
    {
        public CreateChatRequestValidation() 
        {
            RuleFor(e => e.IsGroupChat).NotNull();
            RuleFor(e => e.ChatMembers).NotEmpty();
        }
    }
}
