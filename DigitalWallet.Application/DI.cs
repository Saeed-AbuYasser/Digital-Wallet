using Microsoft.Extensions.DependencyInjection;
namespace DigitalWallet.Application
{
    public static class DI
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DI).Assembly);
            });
            return services;
        }
    }
}
