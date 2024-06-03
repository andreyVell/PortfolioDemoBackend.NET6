using FluentValidation;

namespace WebApi.DTOs.ChatMember
{
    public class CreateChatMemberRequestValidation : AbstractValidator<CreateChatMemberRequest>
    {
        public CreateChatMemberRequestValidation()
        {
            RuleFor(e => e.Type).IsInEnum();
        }
    }
}
