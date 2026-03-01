using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Services
{
    public interface IBillIssuingService
    {
        public Task<BillEntity> IssueAsync(BillEntity entity);
    }
}
