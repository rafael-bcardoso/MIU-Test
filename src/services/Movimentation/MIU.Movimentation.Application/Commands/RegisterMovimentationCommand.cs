using FluentValidation;
using MIU.Core.Messages;
using System;

namespace MIU.Movimentations.Application.Commands
{
    public class RegisterMovimentationCommand : Command
    {
        public string CustomerName { get; set; }
        public string TributeCode { get; set; }
        public string Cpf { get; set; }
        public DateTime MovimentationDate { get; set; }
        public string TributeDescription { get; set; }
        public int TributeAliquot { get; set; }
        public int MovimentationGain { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new RegisterMovimentationValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RegisterMovimentationValidation : AbstractValidator<RegisterMovimentationCommand>
    {
        public RegisterMovimentationValidation()
        {
            RuleFor(x => x.CustomerName)
                .NotEqual(string.Empty)
                .WithMessage("O campo nome do cliente deve ser preenchido");

            RuleFor(x => x.Cpf)
                .NotEqual(string.Empty)
                .WithMessage("O campo nome do cliente deve ser preenchido");

            RuleFor(x => x.TributeCode)
                .NotEqual(string.Empty)
                .WithMessage("O campo nome do cliente deve ser preenchido");

            RuleFor(x => x.Cpf)
                .Must(HasValidCpf)
                .WithMessage("O cpf informado é inválido");
        }

        protected static bool HasValidCpf(string cpf)
        {
            return Core.ValueObjects.CPF.IsValid(cpf);
        }
    }
}
