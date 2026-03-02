using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using MediatR;

namespace DigitalWallet.Application.CQRS.Queries.Transactions
{
    public record ReadAllTransactionsQuery() : IRequest<IEnumerable<TransactionEntity>>;
    public class ReadAllTransactionsQueryHandler(ITransactionRepository transactionRepository) : IRequestHandler<ReadAllTransactionsQuery, IEnumerable<TransactionEntity>>
    {
        public async Task<IEnumerable<TransactionEntity>> Handle(ReadAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await transactionRepository.ReadAllTransactionsAsync();
        }
    }
}