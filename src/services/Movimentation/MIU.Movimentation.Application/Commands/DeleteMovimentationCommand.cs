using FluentValidation;
using MIU.Core.Messages;
using System;

namespace MIU.Movimentations.Application.Commands
{
    public class DeleteMovimentationCommand : Command
    {
        public Guid Id { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new DeleteMovimentationValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class DeleteMovimentationValidation : AbstractValidator<DeleteMovimentationCommand>
    {
        public DeleteMovimentationValidation()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("O Id deve ser preenchido");
        }
    }
}
