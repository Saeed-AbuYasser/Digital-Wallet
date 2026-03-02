using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using MediatR;

namespace DigitalWallet.Application.CQRS.Queries.Transactions
{
    public record ReadAllTransactionsQuery() : IRequest<IEnumerable<TransactionEntity>>;
    public class ReadAllTransactionsQueryHandler : IRequestHandler<ReadAllTransactionsQuery, IEnumerable<TransactionEntity>>
    {
        private readonly ITransactionRepository _transactionRepository;
        public ReadAllTransactionsQueryHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<IEnumerable<TransactionEntity>> Handle(ReadAllTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _transactionRepository.ReadAllTransactionsAsync();
        }
    }
}