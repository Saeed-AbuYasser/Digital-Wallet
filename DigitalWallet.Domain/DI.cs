using Microsoft.Extensions.DependencyInjection;

namespace DigitalWallet.Domain
{
    public static class DI
    {
        public static IServiceCollection AddDomainDI(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DI).Assembly);
            });
            return services;
        }
    }
}
