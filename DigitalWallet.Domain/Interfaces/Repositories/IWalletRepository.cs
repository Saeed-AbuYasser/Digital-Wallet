using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface IWalletRepository
    {
        public Task<IEnumerable<WalletEntity>> ReadAllWalletsAsync();
        public Task<WalletEntity> ReadWalletByIdAsync(Guid Id);
        public Task<WalletEntity> ReadBillerWalletByBillerIdAsync(Guid BillerId);
        public Task<WalletEntity> ReadWalletByBillTypeIdAsync(Guid BillTypeId);
        public Task<WalletEntity> ReadWalletByFrequentBillTypeIdAsync(Guid FrequentBillTypeId);
        public Task<WalletEntity> ReadWalletByNameAsync(string Holder);
        public Task<bool> WalletExistsByIdAsync(Guid Id);
        internal Task<WalletEntity> UpdateWalletAsync(WalletEntity walletEntity);
        public Task<Guid> DeleteWalletByIdAsync(Guid Id);
        public Task<WalletEntity> CreateWalletAsync(WalletEntity walletEntity);
        
    }
}
