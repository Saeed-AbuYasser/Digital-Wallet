using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface IBillRepository
    {
        public Task<IEnumerable<BillEntity>> ReadAllBillsAsync();
        public Task<IEnumerable<BillEntity>> ReadBillsByWalletIdAsync(Guid WalletId);
        public Task<IEnumerable<BillEntity>> ReadPaidBillsByWalletIdAsync(Guid WalletId);
        public Task<IEnumerable<BillEntity>> ReadUnPaidBillsByWalletIdAsync(Guid WalletId);
        public Task<BillEntity> ReadBillByIdAsync(Guid Id);
        internal Task<BillEntity> MarkBillAsPaidAsync(BillEntity billEntity);
        public Task<BillEntity> CreateBillAsync(BillEntity billEntity);


    }
}
