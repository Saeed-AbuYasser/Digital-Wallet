using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Presistence;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Infrastructure.Data.Services
{
    public class WithdrawService(IUnitOfWork unitOfWork) : IWithdrawService
    {
        public async Task WithdrawAsync(Guid walletId, decimal amount)
        {
            if (amount <1) throw new DataCorrectnessViolationException("Withdraw");
            unitOfWork.BeginTransaction();
            try
            {
                //get the wallet by id
                var walletRepo = unitOfWork.Repository<IWalletRepository>();
                var wallet = await walletRepo.ReadWalletByIdAsync(walletId);

                //update
                wallet = wallet with { Balance = wallet.Balance - amount };

                if (wallet.Balance < 0) throw new ArgumentException($"Wallet [{walletId}] has no enough balance to complete the process.");
                //save
                await walletRepo.UpdateWalletAsync(wallet);


                //Log the operation:
                await unitOfWork.Repository<ITransactionRepository>().CreateTransactionAsync
                    (new TransactionEntity(Guid.Empty, walletId, amount, Domain.Enums.TransactionTypeEnum.Withdraw, DateTime.Now));

                unitOfWork.Commit();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
