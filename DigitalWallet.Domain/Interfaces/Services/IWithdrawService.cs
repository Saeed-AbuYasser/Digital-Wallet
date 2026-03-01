
namespace DigitalWallet.Domain.Interfaces.Services
{
    public interface IWithdrawService
    {
        public Task WithdrawAsync(Guid walletId, decimal amount);

    }
}
