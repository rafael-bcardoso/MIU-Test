using Microsoft.Extensions.DependencyInjection;
using MIU.Core.Mediator;
using MIU.Movimentations.Domain.Repositories;
using MIU.Movimentations.Infra;
using MIU.Movimentations.Infra.Repositories;

namespace MIU.Movimentations.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfig(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            services.AddScoped<IMovimentationRepository, MovimentationRepository>();

            services.AddScoped<MovimentationContext>();
        }
    }
}
