using DigitalWallet.Application.DTOs.Billers;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Application.CQRS.Queries.Billers
{
    public record ReadAllBillersQuery():IRequest<IEnumerable<ReadBillerDTO>>;
    public class ReadAllBillersQueryHandler(IBillerRepository billerRepository) : IRequestHandler<ReadAllBillersQuery, IEnumerable<ReadBillerDTO>>
    {
        public async Task<IEnumerable<ReadBillerDTO>> Handle(ReadAllBillersQuery request, CancellationToken cancellationToken)
        {
            var result = await billerRepository.ReadAllBillersAsync();
            return result.Select(biller => new ReadBillerDTO(biller.Id, biller.WalletId, biller.Name));
        }
    }
}
