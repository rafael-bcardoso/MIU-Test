using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIU.Core.Data;
using MIU.Movimentations.Application.CommandHandlers;
using MIU.Movimentations.Application.Commands;
using MIU.Movimentations.Domain.Entities;
using MIU.Movimentations.Domain.Repositories;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MIU.Movimentations.Application.Tests
{
    [TestClass]
    public class MovimentationCommandHandlerTest
    {
        private readonly Mock<IMovimentationRepository> _movimentationRepository;

        public MovimentationCommandHandlerTest()
        {
            _movimentationRepository = new Mock<IMovimentationRepository>();
        }

        [TestMethod]
        public void Deve_Retonar_Invalido_Quando_Um_RegisterMovimentationCommand_Nao_Estiver_Correto()
        {
            var registerMovimentationCommand = new RegisterMovimentationCommand()
            {
                Cpf = "123123123123"
            };

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(registerMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(false, result.Result.IsValid);
        }

        [TestMethod]
        public void Deve_Retonar_IsValid_True_Ao_Registrar_Uma_Movimentacao()
        {
            var registerMovimentationCommand = new RegisterMovimentationCommand()
            {
                Cpf = "427.642.200-06"
            };

            _movimentationRepository.Setup(x => x.UnitOfWork)
                .Returns(new MovimentationContextFake());

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(registerMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(true, result.Result.IsValid);
        }

        [TestMethod]
        public void Deve_Retonar_Invalido_Quando_Um_DeleteMovimentationCommand_Nao_Estiver_Correto()
        {
            var deleteMovimentationCommand = new DeleteMovimentationCommand()
            {
                Id = Guid.Empty
            };

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(deleteMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(false, result.Result.IsValid);
        }

        [TestMethod]
        public void Deve_Retonar_Erro_Se_A_Movimentacao_Nao_For_Encontrada_Ao_Deltarmos_Uma_Movimentacao()
        {
            var id = Guid.NewGuid();
            var deleteMovimentationCommand = new DeleteMovimentationCommand()
            {
                Id = id
            };

            _movimentationRepository.Setup(x => x.GetMovimentationById(id));

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(deleteMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(1, result.Result.Errors.Count);
        }

        [TestMethod]
        public void Deve_Retonar_IsValid_True_Ao_Deletarmos_Uma_Movimentacao()
        {
            var id = Guid.NewGuid();
            var deleteMovimentationCommand = new DeleteMovimentationCommand()
            {
                Id = id
            };
            var movimentationFake = GetMovimentationFake();

            _movimentationRepository.Setup(x => x.UnitOfWork)
                .Returns(new MovimentationContextFake());

            _movimentationRepository.Setup(x => x.GetMovimentationById(id))
                .Returns(movimentationFake);

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(deleteMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(true, result.Result.IsValid);
        }

        [TestMethod]
        public void Deve_Retonar_Erro_Se_A_Movimentacao_Nao_For_Encontrada_Ao_Atualizarmos_Uma_Movimentacao()
        {
            var id = Guid.NewGuid();
            var deleteMovimentationCommand = new UpdateMovimentationCommand()
            {
                Id = id
            };

            _movimentationRepository.Setup(x => x.GetMovimentationById(id));

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(deleteMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(1, result.Result.Errors.Count);
        }

        [TestMethod]
        public void Deve_Retonar_IsValid_True_Ao_Atualizarmos_Uma_Movimentacao()
        {
            var id = Guid.NewGuid();
            var updateMovimentationCommand = new UpdateMovimentationCommand()
            {
                Id = id,
                Cpf = "427.642.200-06"
            };
            
            var movimentationFake = GetMovimentationFake();

            _movimentationRepository.Setup(x => x.UnitOfWork)
                .Returns(new MovimentationContextFake());

            _movimentationRepository.Setup(x => x.GetMovimentationById(id))
                .Returns(movimentationFake);

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(updateMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(true, result.Result.IsValid);
        }

        [TestMethod]
        public void Deve_Retonar_Invalido_Quando_Um_UpdateMovimentationCommand_Nao_Estiver_Correto()
        {
            var updateMovimentationCommand = new UpdateMovimentationCommand()
            {
                Id = Guid.Empty
            };

            var movimentationCommandHandler = new MovimentationCommandHandler(_movimentationRepository.Object);
            var result = movimentationCommandHandler.Handle(updateMovimentationCommand, It.IsAny<CancellationToken>());

            Assert.AreEqual(false, result.Result.IsValid);
        }

        private Movimentation GetMovimentationFake()
        {
            return new Movimentation("","", "427.642.200-06", DateTime.Now, "",1,1);
        }
    }

    public class MovimentationContextFake : IUnitOfWork
    {
        public async Task<bool> Commit()
        {
            return await Task.FromResult(true);
        }
    }
}
