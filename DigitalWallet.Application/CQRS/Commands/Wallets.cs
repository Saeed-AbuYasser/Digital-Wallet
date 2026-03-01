using DigitalWallet.Application.DTOs.Wallets;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Domain.Interfaces.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Application.CQRS.Commands.Wallets
{
    public record CreateWalletCommand(CreateWalletDTO Wallet) : IRequest<ReadWalletDTO>;
    public record WithdrawCommand(Guid WalletId, decimal Amount) : IRequest;
    public record DepositCommand(Guid WalletId, decimal Amount) : IRequest;
    public record DeleteWalletByIdCommand(Guid Id) : IRequest<Guid>;
    public record TransferMoneyCommand(Guid SourceWalletId, Guid DestinationWalletId, decimal Amount) : IRequest;
    public class CreateWalletCommandHandler(IWalletRepository walletRepository) : IRequestHandler<CreateWalletCommand, ReadWalletDTO>
    {
        public async Task<ReadWalletDTO> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            WalletEntity walletEntity = new WalletEntity(Guid.Empty, request.Wallet.Name, request.Wallet.InitialBalance);
            var wallet = await walletRepository.CreateWalletAsync(walletEntity);
            return new ReadWalletDTO(
                wallet.Id,
                wallet.Holder,
                wallet.Balance
            );
        }
    }
    public class WithdrawCommandHandler(IWithdrawService withdrawService) : IRequestHandler<WithdrawCommand>
    {
        public async Task Handle(WithdrawCommand request, CancellationToken cancellationToken)
        {
            await withdrawService.WithdrawAsync(request.WalletId, request.Amount);
        }
    }
    public class DepositCommandHandler(IDepositService DepositService) : IRequestHandler<DepositCommand>
    {
        public async Task Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            await DepositService.DepositAsync(request.WalletId, request.Amount);
        }
    }
    public class DeleteWalletByIdCommandHandler(IWalletRepository walletRepository) : IRequestHandler<DeleteWalletByIdCommand, Guid>
    {
        public async Task<Guid> Handle(DeleteWalletByIdCommand request, CancellationToken cancellationToken)
        {
            return await walletRepository.DeleteWalletByIdAsync(request.Id);
        }
    }
    public class TransferMoneyCommandHandler(ITransferMoneyService transferService) : IRequestHandler<TransferMoneyCommand>
    {
        public async Task Handle(TransferMoneyCommand request, CancellationToken cancellationToken)
        {
            await transferService.TransferAsync(request.SourceWalletId, request.DestinationWalletId, request.Amount);
        }
    }
}
