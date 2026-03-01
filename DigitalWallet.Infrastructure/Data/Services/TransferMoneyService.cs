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
    public class TransferMoneyService(IUnitOfWork unitOfWork) : ITransferMoneyService
    {
        public async Task TransferAsync(Guid SourceWalletId, Guid DestinationWalletId, decimal amount)
        {
            //check the correctness of amount value.
            if( amount < 1) throw new DataCorrectnessViolationException("Transfer");
            //begin transaction.
            unitOfWork.BeginTransaction();
            try
            {
                //load repos.
                var walletRepo = unitOfWork.Repository<IWalletRepository>();
                var transferRepo = unitOfWork.Repository<ITransferRepository>();

                //read wallets from repo.
                var SourceWallet = await walletRepo.ReadWalletByIdAsync(SourceWalletId);
                //check source wallet has enough money.
                if (SourceWallet.Balance < amount) throw new ArgumentException($"Source Wallet [{SourceWallet.Holder}] has no enough balance to complete the process.");

                var DestinationWallet = await walletRepo.ReadWalletByIdAsync(DestinationWalletId);

                //apply updates on wallets
                SourceWallet = SourceWallet with { Balance = SourceWallet.Balance - amount };
                DestinationWallet = DestinationWallet with { Balance = DestinationWallet.Balance + amount };

                //save updates
                await walletRepo.UpdateWalletAsync(SourceWallet);
                await walletRepo.UpdateWalletAsync(DestinationWallet);

                //log transfer (it will automatically generate a new Id inside).
                await transferRepo.CreateTransferAsync(new(Guid.Empty, SourceWalletId, DestinationWalletId, amount, DateTime.Now));

                unitOfWork.Commit();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
                //notic that each method handle its own problems then throw them. You don't have to
                // handle any thing that it's expected to happen inside any method you call!!.
            }
        }
    }
}
