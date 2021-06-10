using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIU.Core.Mediator;
using MIU.Movimentations.API.Controllers;
using MIU.Movimentations.Application.Commands;
using MIU.Movimentations.Domain.Entities;
using MIU.Movimentations.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MIU.Movimentations.Api.Tests
{
    [TestClass]
    public class MovimentationControllerTest
    {
        private readonly Mock<IMovimentationRepository> _movimentationRepository;
        private readonly Mock<IMediatorHandler> _mediatorHandler;

        public MovimentationControllerTest()
        {
            _movimentationRepository = new Mock<IMovimentationRepository>();
            _mediatorHandler = new Mock<IMediatorHandler>();
        }

        [TestMethod]
        public void Deve_Retornar_Status_Code_200()
        {
            var movimentationsFake = GetMovimentationsFake();

            _movimentationRepository.Setup(x => x.GetMovimentations())
                .Returns(movimentationsFake);

            var movimentationController = new MovimentationController(_movimentationRepository.Object, _mediatorHandler.Object);
            var result = movimentationController.Get().Result as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Deve_Retornar_Status_Code_200_Quando_Uma_Movimentacao_Criada_Com_Sucesso()
        {
            var registerMovimentationCommand = new RegisterMovimentationCommand() 
            {
                Cpf = "976.153.660-20"
            };

            var movimentationsFake = GetMovimentationsFake();

            _movimentationRepository.Setup(x => x.GetMovimentations())
                .Returns(movimentationsFake);

            _mediatorHandler.Setup(x => x.PublisherCommand(registerMovimentationCommand))
                .ReturnsAsync(new ValidationResult());

            var movimentationController = new MovimentationController(_movimentationRepository.Object, _mediatorHandler.Object);
            var result = movimentationController.CreateMovimentation(registerMovimentationCommand).Result as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Deve_Retornar_Uma_Lista_Com_Os_Erros_Quando_O_Command_Nao_Estiver_Preenchido_Corretamente()
        {
            var registerMovimentationCommand = new RegisterMovimentationCommand()
            {
                Cpf = "122.222.222-22"
            };

            var movimentationsFake = GetMovimentationsFake();
            var validationFailuresFake = GetValidationFailuresFake();

            _movimentationRepository.Setup(x => x.GetMovimentations())
                .Returns(movimentationsFake);

            _mediatorHandler.Setup(x => x.PublisherCommand(registerMovimentationCommand))
                .Returns(validationFailuresFake);

            var movimentationController = new MovimentationController(_movimentationRepository.Object, _mediatorHandler.Object);
            var result = movimentationController.CreateMovimentation(registerMovimentationCommand).Result as ObjectResult;
            var validationProblemDetails = (ValidationProblemDetails)result.Value;

            Assert.AreEqual(1, validationProblemDetails.Errors.Count());
        }

        [TestMethod]
        public void Deve_Retornar_Status_Code_200_Quando_Deletar_Uma_Movimentacao()
        {
            var deleteMovimentationCommand = new DeleteMovimentationCommand()
            {
                Id = Guid.NewGuid()
            };

            var movimentationsFake = GetMovimentationsFake();

            _movimentationRepository.Setup(x => x.GetMovimentations())
                .Returns(movimentationsFake);

            _mediatorHandler.Setup(x => x.PublisherCommand(It.IsAny<DeleteMovimentationCommand>()))
                .ReturnsAsync(new ValidationResult());

            var movimentationController = new MovimentationController(_movimentationRepository.Object, _mediatorHandler.Object);
            var result = movimentationController.DeleteMovimentation(Guid.NewGuid().ToString()).Result as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Deve_Retornar_Uma_Lista_Com_Os_Erros_Quando_O_DeleteMovimentationCommand_Nao_Estiver_Preenchido_Corretamente()
        {
            var deleteMovimentationCommand = new DeleteMovimentationCommand()
            {
                Id = Guid.NewGuid()
            };

            var movimentationsFake = GetMovimentationsFake();
            var validationFailuresFake = GetValidationFailuresFake();

            _movimentationRepository.Setup(x => x.GetMovimentations())
                .Returns(movimentationsFake);

            _mediatorHandler.Setup(x => x.PublisherCommand(It.IsAny<DeleteMovimentationCommand>()))
                .Returns(validationFailuresFake);

            var movimentationController = new MovimentationController(_movimentationRepository.Object, _mediatorHandler.Object);
            var result = movimentationController.DeleteMovimentation(Guid.NewGuid().ToString()).Result as ObjectResult;
            var validationProblemDetails = (ValidationProblemDetails)result.Value;

            Assert.AreEqual(1, validationProblemDetails.Errors.Count());
        }

        [TestMethod]
        public void Deve_Retornar_Status_Code_200_Quando_Uma_Movimentacao_For_Atualizada()
        {
            var updateMovimentationCommand = new UpdateMovimentationCommand()
            {
                Cpf = "976.153.660-20"
            };

            var movimentationsFake = GetMovimentationsFake();

            _movimentationRepository.Setup(x => x.GetMovimentations())
                .Returns(movimentationsFake);

            _mediatorHandler.Setup(x => x.PublisherCommand(It.IsAny<UpdateMovimentationCommand>()))
                .ReturnsAsync(new ValidationResult());

            var movimentationController = new MovimentationController(_movimentationRepository.Object, _mediatorHandler.Object);
            var result = movimentationController.UpdateMovimentation(updateMovimentationCommand).Result as ObjectResult;

            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void Deve_Retornar_Uma_Lista_Com_Os_Erros_Quando_O_UpdateMovimentationCommand_Nao_Estiver_Preenchido_Corretamente()
        {
            var updateMovimentationCommand = new UpdateMovimentationCommand()
            {
                Id = Guid.NewGuid()
            };

            var movimentationsFake = GetMovimentationsFake();
            var validationFailuresFake = GetValidationFailuresFake();

            _movimentationRepository.Setup(x => x.GetMovimentations())
                .Returns(movimentationsFake);

            _mediatorHandler.Setup(x => x.PublisherCommand(It.IsAny<UpdateMovimentationCommand>()))
                .Returns(validationFailuresFake);

            var movimentationController = new MovimentationController(_movimentationRepository.Object, _mediatorHandler.Object);
            var result = movimentationController.UpdateMovimentation(updateMovimentationCommand).Result as ObjectResult;
            var validationProblemDetails = (ValidationProblemDetails)result.Value;

            Assert.AreEqual(1, validationProblemDetails.Errors.Count());
        }

        private async Task<IList<Movimentation>> GetMovimentationsFake()
        {
            var movimentations = new List<Movimentation>()
            {
                new Movimentation("","","976.153.660-20",DateTime.Now,"",1,1),
                new Movimentation("","","427.642.200-06",DateTime.Now,"",1,1),
            };

            return await Task.FromResult(movimentations);
        }

        private async Task<ValidationResult> GetValidationFailuresFake()
        {
            var validationFailures = new List<ValidationFailure>
            {
                new ValidationFailure("", "O cpf informado é inválido")
            };

            var validationResult = new ValidationResult(validationFailures);

            
            return await Task.FromResult(validationResult);
        }
    }
}
