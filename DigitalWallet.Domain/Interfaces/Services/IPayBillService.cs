
namespace DigitalWallet.Domain.Interfaces.Services
{
    public interface IPayBillService
    {
        public Task PayBillAsync(Guid billId);
    }
}
