using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Presistence;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Domain.Interfaces.Services;
using DigitalWallet.Infrastructure.Helper;
using Microsoft.Data.SqlClient;

namespace DigitalWallet.Infrastructure.Data.Services
{
    public class BillIssuingService(IUnitOfWork unitOfWork) : IBillIssuingService
    {
        public async Task<BillEntity> IssueAsync(BillEntity billEntity)
        {
            if (billEntity.Amount < 1) throw new DataCorrectnessViolationException("Bill");
            try
            {
                //get the bill type amount by id
                var billTypeRepo = unitOfWork.Repository<IBillTypeRepository>();
                var billRepo = unitOfWork.Repository<IBillRepository>();
                var amount = await billTypeRepo.GetBillTypeAmountByIdAsync(billEntity.BillTypeId);

                //create the bill with the amount
                billEntity = billEntity with { Amount = amount };
                // This method will create the Bill, then will send back the saved bill with complete data (including WalletHolder name, BillType name)
                billEntity = await billRepo.CreateBillAsync(billEntity);
            }
            catch
            {
                throw;
            }
            return billEntity;
        }
    }
}
