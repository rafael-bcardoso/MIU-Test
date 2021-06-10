using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using MIU.Core.Data;
using MIU.Core.Domain;
using MIU.Core.Mediator;
using MIU.Core.Messages;
using MIU.Movimentations.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MIU.Movimentations.Infra
{
    public class MovimentationContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public MovimentationContext(DbContextOptions<MovimentationContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<Movimentation> Movimentations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("VARCHAR(250)");

            foreach (var relationship in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Cascade;

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MovimentationContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var successful = await base.SaveChangesAsync() > 0;

            if (successful) await _mediatorHandler.PublisherEvent(this);

            return successful;
        }
    }

    public static class MediatorExtension
    {
        public static async Task PublisherEvent<T>(this IMediatorHandler mediator, T context) where T : DbContext
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Notifications != null && x.Entity.Notifications.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notifications)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublisherEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}
