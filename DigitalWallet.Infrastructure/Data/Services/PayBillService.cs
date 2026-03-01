using DigitalWallet.Domain.Interfaces.Presistence;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Domain.Interfaces.Services;
using DigitalWallet.Infrastructure.Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Infrastructure.Data.Services;

public class PayBillService(IUnitOfWork unitOfWork):IPayBillService
{
    public async Task PayBillAsync(Guid billId)
    {
        // We use BeginTransaction to start the session
        unitOfWork.BeginTransaction();
        try
        {
            // The UoW creates the repo and passes the connection/transaction for you!
            var walletRepo = unitOfWork.Repository<IWalletRepository>();
            var billRepo = unitOfWork.Repository<IBillRepository>();

            // Now retrieve the wallet and bill, update the wallet balance and mark the bill as paid
            var bill = await billRepo.ReadBillByIdAsync(billId);
            var Clientwallet = await walletRepo.ReadWalletByIdAsync(bill.WalletId);
            var Billerwallet = await walletRepo.ReadWalletByBillTypeIdAsync(bill.BillTypeId);

            Clientwallet = Clientwallet with { Balance = Clientwallet.Balance - (decimal)bill.Amount! };
            await walletRepo.UpdateWalletAsync(Clientwallet);
            Billerwallet = Billerwallet with { Balance = Billerwallet.Balance + (decimal)bill.Amount! };
            await walletRepo.UpdateWalletAsync(Billerwallet);
            // This method will update the Bill to marke it as paid, then will send back the updated bill with complete data (including the PaidAt date, WalletHolder name, BillType name)
            await billRepo.MarkBillAsPaidAsync(bill);

            unitOfWork.Commit();
        }
        catch
        {
            unitOfWork.Rollback();
            throw;
        }
    }
}