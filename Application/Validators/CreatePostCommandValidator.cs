using Application.Commands;
using Application.ValidatorMessages;
using FluentValidation;

namespace Application.Validators;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(ValidatorMessage.UserIdRequired);
        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(1_000)
            .WithMessage($"{ValidatorMessage.ContentRequired} and {ValidatorMessage.ContentMaxLength}");
    }
}