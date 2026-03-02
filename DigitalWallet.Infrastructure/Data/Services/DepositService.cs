using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Presistence;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Domain.Interfaces.Services;
using DigitalWallet.Infrastructure.Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Infrastructure.Data.Services
{
    //إيداع
    public class DepositService(IUnitOfWork unitOfWork) : IDepositService
    {
        public async Task DepositAsync(Guid walletId, decimal amount)
        {
            if (amount < 1) throw new DataCorrectnessViolationException("Deposit");
            unitOfWork.BeginTransaction();
            try
            {
                //get the wallet by id
                var walletRepo = unitOfWork.Repository<IWalletRepository>();
                var wallet = await walletRepo.ReadWalletByIdAsync(walletId);
                //update
                wallet = wallet with { Balance = wallet.Balance + amount };

                //save
                await walletRepo.UpdateWalletAsync(wallet);

                //Log the operation:
                await unitOfWork.Repository<ITransactionRepository>().CreateTransactionAsync
                    (new TransactionEntity(Guid.Empty,walletId,amount,Domain.Enums.TransactionTypeEnum.Deposit,DateTime.Now));

                unitOfWork.Commit();
            }
            catch
            {
                unitOfWork.Rollback();
            }

        }
    }
}
