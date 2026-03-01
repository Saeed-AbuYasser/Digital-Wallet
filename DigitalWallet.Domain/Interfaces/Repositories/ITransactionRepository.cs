using DigitalWallet.Domain.Entities;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface ITransactionRepository
    {
        public Task<IEnumerable<TransactionEntity>> ReadAllTransactionsAsync();
        public Task<IEnumerable<TransactionEntity>> ReadTransactionsByWalletIdAsync(Guid WalletId);
        internal Task<TransactionEntity> CreateTransactionAsync(TransactionEntity transactionEntity);

    }
}
