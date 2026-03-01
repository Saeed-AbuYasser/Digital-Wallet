
namespace DigitalWallet.Domain.Interfaces.Services
{
    public interface ITransferMoneyService
    {
        public Task TransferAsync(Guid SourceWalletId, Guid DestinationWalletId, decimal amount);
    }
}
