using FluentValidation;
using MIU.Core.Messages;
using System;

namespace MIU.Movimentations.Application.Commands
{
    public class UpdateMovimentationCommand : Command
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string TributeCode { get; set; }
        public string Cpf { get; set; }
        public DateTime MovimentationDate { get; set; }
        public string TributeDescription { get; set; }
        public int TributeAliquot { get; set; }
        public int MovimentationGain { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateMovimentationValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateMovimentationValidation : AbstractValidator<UpdateMovimentationCommand>
    {
        public UpdateMovimentationValidation()
        {
            RuleFor(x => x.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("O campo Id deve ser preenchido");

            RuleFor(x => x.CustomerName)
                .NotEqual(string.Empty)
                .WithMessage("O campo nome do cliente deve ser preenchido");

            RuleFor(x => x.Cpf)
                .NotEqual(string.Empty)
                .WithMessage("O campo nome do cliente deve ser preenchido");

            RuleFor(x => x.TributeCode)
                .NotEqual(string.Empty)
                .WithMessage("O campo nome do cliente deve ser preenchido");
        }
    }
}
