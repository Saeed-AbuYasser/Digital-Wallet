
namespace DigitalWallet.Domain.Interfaces.Services
{
    public interface IDepositService
    {
        public Task DepositAsync(Guid walletId, decimal amount);
    }
}
