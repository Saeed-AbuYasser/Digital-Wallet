using DigitalWallet.Application;
using DigitalWallet.Infrastructure;

namespace DigitalWallet.API
{
    public static class DI
    {
        public static IServiceCollection AddApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DI).Assembly);
            });

            services
                .AddApplicationDI()
                .AddInfrastructureDI(configuration);
            //we added those both for now. If we ever need to add the domain DI, we can just add it here as well.
            return services;
        }
    }
}
