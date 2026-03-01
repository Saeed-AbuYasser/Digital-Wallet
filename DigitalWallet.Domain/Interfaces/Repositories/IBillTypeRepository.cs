using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface IBillTypeRepository
    {
        public Task<IEnumerable<BillTypeEntity>> ReadAllBillTypesAsync();
        public Task<IEnumerable<BillTypeEntity>> ReadBillTypesByBillerIdAsync(Guid BillerId);
        public Task<BillTypeEntity> ReadBillTypeByIdAsync(Guid id);
        public Task<BillTypeEntity> CreateBillTypeAsync(BillTypeEntity billTypeEntity);
        public Task<BillTypeEntity> UpdateBillTypeAsync(BillTypeEntity billTypeEntity);
        public Task<decimal> GetBillTypeAmountByIdAsync(Guid Id);
        public Task DeleteBillTypeAsync(Guid Id);
    }
}
