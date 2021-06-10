using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MIU.Core.Mediator;
using MIU.Movimentations.Domain.Entities;
using MIU.Movimentations.Infra.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MIU.Movimentations.Infra.Tests
{
    [TestClass]
    public class MovimentationRepositoryTest
    {
        private readonly Mock<MovimentationContext> _movimentationContext;

        public MovimentationRepositoryTest()
        {
            _movimentationContext = new Mock<MovimentationContext>();
        }

        [TestMethod]
        public void Deve_Retornar_Uma_Lista_Com_Todas_As_Movimentacoes()
        {
            var options = new DbContextOptionsBuilder<MovimentationContext>()
                            .UseInMemoryDatabase(databaseName: "MIU")                            
                            .Options;

            using (var context = new MovimentationContext(options, It.IsAny<IMediatorHandler>()))
            {
                context.Movimentations.Add(new Movimentation("", "", "588.847.080-52", DateTime.Now, "", 1, 1));
                context.SaveChanges();
            }

            using (var context = new MovimentationContext(options, It.IsAny<IMediatorHandler>()))
            {
                var movimentationRepository = new MovimentationRepository(context);
                var movimentations = movimentationRepository.GetMovimentations();

                Assert.AreEqual(1, movimentations.Result.Count);
                ClearDatabase(context);
            }
        }

        [TestMethod]
        public void Deve_Adicionar_Uma_Movimentacao()
        {
            var options = new DbContextOptionsBuilder<MovimentationContext>()
                            .UseInMemoryDatabase(databaseName: "MIU")
                            .Options;

            using (var context = new MovimentationContext(options, It.IsAny<IMediatorHandler>()))
            {
                var movimentationRepository = new MovimentationRepository(context);
                movimentationRepository.AddMovimentation(new Movimentation("", "", "588.847.080-52", DateTime.Now, "", 1, 1));
                context.SaveChanges();
                var movimentations = movimentationRepository.GetMovimentations();

                Assert.AreEqual(1, movimentations.Result.Count);
                ClearDatabase(context);
            }
        }

        [TestMethod]
        public void Deve_Atualizar_Uma_Movimentacao()
        {
            var options = new DbContextOptionsBuilder<MovimentationContext>()
                            .UseInMemoryDatabase(databaseName: "MIU")
                            .Options;

            var movimentation = new Movimentation("", "", "588.847.080-52", DateTime.Now, "", 1, 1);

            using (var context = new MovimentationContext(options, It.IsAny<IMediatorHandler>()))
            {
                var movimentationRepository = new MovimentationRepository(context);
                movimentationRepository.AddMovimentation(movimentation);
                context.SaveChanges();
                movimentationRepository.UpdateMovimentation(movimentation);
                context.SaveChanges();
                var movimentations = movimentationRepository.GetMovimentations();

                Assert.AreEqual(1, movimentations.Result.Count);
                ClearDatabase(context);
            }
        }

        [TestMethod]
        public void Deve_Remover_Uma_Movimentacao()
        {
            var options = new DbContextOptionsBuilder<MovimentationContext>()
                            .UseInMemoryDatabase(databaseName: "MIU")
                            .Options;

            var movimentation = new Movimentation("", "", "588.847.080-52", DateTime.Now, "", 1, 1);

            using (var context = new MovimentationContext(options, It.IsAny<IMediatorHandler>()))
            {
                var movimentationRepository = new MovimentationRepository(context);
                movimentationRepository.AddMovimentation(movimentation);
                context.SaveChanges();
                movimentationRepository.RemoveMovimentation(movimentation);
                context.SaveChanges();
                var movimentations = movimentationRepository.GetMovimentations();

                Assert.AreEqual(0, movimentations.Result.Count);
                ClearDatabase(context);
            }
        }

        [TestMethod]
        public void Deve_Retornar_Uma_Movimentacao_Pelo_Id()
        {
            var options = new DbContextOptionsBuilder<MovimentationContext>()
                            .UseInMemoryDatabase(databaseName: "MIU")
                            .Options;

            var movimentation = new Movimentation("", "", "588.847.080-52", DateTime.Now, "", 1, 1);

            using (var context = new MovimentationContext(options, It.IsAny<IMediatorHandler>()))
            {
                var movimentationRepository = new MovimentationRepository(context);
                movimentationRepository.AddMovimentation(movimentation);
                context.SaveChanges();
                var movimentationSaved = movimentationRepository.GetMovimentationById(movimentation.Id);

                Assert.AreEqual(movimentation.Id, movimentationSaved.Id);
                ClearDatabase(context);
            }
        }

        private List<Movimentation> GetMovimentationsFake()
        {
            return new List<Movimentation>();
        }

        public void ClearDatabase(MovimentationContext context)
        {
            context.Database.EnsureDeleted();
        }

    }
}
