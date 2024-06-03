using FluentValidation;

namespace WebApi.DTOs.ChatMessage
{
    public class CreateChatMessageRequestValidation : AbstractValidator<CreateChatMessageRequest>
    {
        public CreateChatMessageRequestValidation()
        {

            RuleFor(e => e.Text)
                .Must(x => !string.IsNullOrWhiteSpace(x))
                .When(e => e.AttachedFiles?.Count == 0)
                .WithMessage("Нельзя отправить пустое сообщение");
            RuleFor(e => e.AttachedFiles)
                .Must(x => x!= null && x.Count>0)
                .When(e => string.IsNullOrWhiteSpace(e.Text))
                .WithMessage("Нельзя отправить пустое сообщение");
            RuleFor(e => e.OwnerId).NotEmpty().NotEqual(Guid.Empty);
            RuleFor(e => e.ChatId).NotEmpty().NotEqual(Guid.Empty);
        }
    }
}
