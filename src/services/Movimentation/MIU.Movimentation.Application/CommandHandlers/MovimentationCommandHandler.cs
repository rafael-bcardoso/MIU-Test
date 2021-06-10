using FluentValidation.Results;
using MediatR;
using MIU.Core.Messages;
using MIU.Movimentations.Application.Commands;
using MIU.Movimentations.Domain.Entities;
using MIU.Movimentations.Domain.Repositories;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MIU.Movimentations.Application.CommandHandlers
{
    public class MovimentationCommandHandler : CommandHandler,
        IRequestHandler<RegisterMovimentationCommand, ValidationResult>,
        IRequestHandler<DeleteMovimentationCommand, ValidationResult>,
        IRequestHandler<UpdateMovimentationCommand, ValidationResult>
    {
        private readonly IMovimentationRepository _movimentationRepository;

        public MovimentationCommandHandler(IMovimentationRepository movimentationRepository)
        {
            _movimentationRepository = movimentationRepository;
        }

        public async Task<ValidationResult> Handle(RegisterMovimentationCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
                return message.ValidationResult;

            var movimentation = new Movimentation(message.TributeCode, 
                                                  message.CustomerName, 
                                                  message.Cpf, 
                                                  message.MovimentationDate, 
                                                  message.TributeDescription, 
                                                  message.TributeAliquot, 
                                                  message.MovimentationGain);

            _movimentationRepository.AddMovimentation(movimentation);

            return await SaveData(_movimentationRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(DeleteMovimentationCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid())
                return message.ValidationResult;

            var movimentation = _movimentationRepository.GetMovimentationById(message.Id);

            if(movimentation == null)
            {
                var failures = new List<ValidationFailure> { new ValidationFailure("", "A movimentação não foi encontrada") };
                var validationResult = new ValidationResult(failures);
                return await Task.FromResult(validationResult);
            }

            _movimentationRepository.RemoveMovimentation(movimentation);

            return await SaveData(_movimentationRepository.UnitOfWork);
        }

        public async Task<ValidationResult> Handle(UpdateMovimentationCommand message, CancellationToken cancellationToken)
        {
            var failures = new List<ValidationFailure>();

            if (!message.IsValid())
                return message.ValidationResult;

            var movimentation = _movimentationRepository.GetMovimentationById(message.Id);

            if (movimentation == null)
            {
                failures.Add(new ValidationFailure("", "A movimentação não foi encontrada"));
                var validationResult = new ValidationResult(failures);
                return await Task.FromResult(validationResult);
            }

            movimentation.UpdateMovimentation(message.TributeCode,
                                              message.CustomerName,
                                              message.Cpf,
                                              message.MovimentationDate,
                                              message.TributeDescription,
                                              message.TributeAliquot,
                                              message.MovimentationGain);

            _movimentationRepository.UpdateMovimentation(movimentation);

            return await SaveData(_movimentationRepository.UnitOfWork);
        }
    }
}
