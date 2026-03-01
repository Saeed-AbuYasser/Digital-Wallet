using DigitalWallet.Domain.Interfaces.Presistence;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Domain.Interfaces.Services;
using DigitalWallet.Infrastructure.Data;
using DigitalWallet.Infrastructure.Data.Repositories;
using DigitalWallet.Infrastructure.Data.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Data.Common;
namespace DigitalWallet.Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            services.AddScoped<DbSession>(_ => new DbSession(connectionString));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Repositories
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<IBillRepository, BillRepository>();
            services.AddScoped<IBillTypeRepository, BillTypeRepository>();
            services.AddScoped<IBillerRepository, BillerRepository>();

            //Services
            services.AddScoped<IBillIssuingService, BillIssuingService>();
            services.AddScoped<IPayBillService, PayBillService>();
            services.AddScoped<ITransferMoneyService, TransferMoneyService>();
            services.AddScoped<IDepositService, DepositService>();
            services.AddScoped<IWithdrawService, WithdrawService>();


            //MediatR
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(DI).Assembly);
            });
            return services;
        }
    }
}
