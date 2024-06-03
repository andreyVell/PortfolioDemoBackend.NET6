using FluentValidation;

namespace WebApi.DTOs.ChatMember
{
    public class CreateChatMemberForNewChatRequestValidation : AbstractValidator<CreateChatMemberForNewChatRequest>
    {
        public CreateChatMemberForNewChatRequestValidation()
        {
            RuleFor(e => e.Type).IsInEnum();            
        }
    }
}
