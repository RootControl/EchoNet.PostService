using Application.Commands;
using Application.ValidatorMessages;
using FluentValidation;

namespace Application.Validators;

public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidatorMessage.PostIdRequired);
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(ValidatorMessage.UserIdRequired);
    }
}