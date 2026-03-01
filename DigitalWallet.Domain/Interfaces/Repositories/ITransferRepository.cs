using DigitalWallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface ITransferRepository
    {
        internal Task<TransferEntity> CreateTransferAsync(TransferEntity transferEntity);
        public Task<IEnumerable<TransferEntity>> ReadAllTransfersAsync();
        public Task<IEnumerable<TransferEntity>> ReadTransfersBySourceWalletIdAsync(Guid WalletId);
        public Task<IEnumerable<TransferEntity>> ReadTransfersByDestinationWalletIdAsync(Guid DestinationWalletId);
    }
}
