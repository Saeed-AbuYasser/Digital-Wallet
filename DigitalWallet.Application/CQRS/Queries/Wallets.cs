using DigitalWallet.Application.DTOs.Wallets;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using MediatR;

namespace DigitalWallet.Application.CQRS.Queries.Wallets
{
    public record ReadAllWalletsQuery : IRequest<IEnumerable<ReadWalletDTO>>;
    public class ReadAllWalletsQueryHandler(IWalletRepository walletRepository) : IRequestHandler<ReadAllWalletsQuery, IEnumerable<ReadWalletDTO>>
    {
        public async Task<IEnumerable<ReadWalletDTO>> Handle(ReadAllWalletsQuery request, CancellationToken cancellationToken)
        {
            var walletEntities = await walletRepository.ReadAllWalletsAsync();
            return walletEntities.Select(wallet => new ReadWalletDTO(
                wallet.Id,
                wallet.Holder,
                wallet.Balance
            ));
        }
    }
    public record ReadWalletByIdQuery(Guid Id) : IRequest<ReadWalletDTO>;
    public record ReadWalletByNameQuery(string Name) : IRequest<ReadWalletDTO>;
    public class ReadWalletByIdQueryHandler(IWalletRepository walletRepository) : IRequestHandler<ReadWalletByIdQuery, ReadWalletDTO>
    {
        public async Task<ReadWalletDTO> Handle(ReadWalletByIdQuery request, CancellationToken cancellationToken)
        {
            var walletEntity = await walletRepository.ReadWalletByIdAsync(request.Id);
            return new ReadWalletDTO(
                walletEntity.Id,
                walletEntity.Holder,
                walletEntity.Balance
            );
        }
    }
    public class ReadWalletByNameQueryHandler(IWalletRepository walletRepository) : IRequestHandler<ReadWalletByNameQuery, ReadWalletDTO>
    {
        public async Task<ReadWalletDTO> Handle(ReadWalletByNameQuery request, CancellationToken cancellationToken)
        {
            var walletEntity = await walletRepository.ReadWalletByNameAsync(request.Name);
            return new ReadWalletDTO(
                walletEntity.Id,
                walletEntity.Holder,
                walletEntity.Balance
            );
        }
    }

    public record WalletExistsByIdQuery(Guid Id) : IRequest<bool>;
    public class WalletExistsByIdQueryHandler(IWalletRepository walletRepository) : IRequestHandler<WalletExistsByIdQuery, bool>
    {
        public async Task<bool> Handle(WalletExistsByIdQuery request, CancellationToken cancellationToken)
        {
            return await walletRepository.WalletExistsByIdAsync(request.Id);
        }
    }



}
