using DigitalWallet.Application.DTOs.BillTypes;
using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using DigitalWallet.Domain.Interfaces.Repositories;
namespace DigitalWallet.Application.CQRS.Queries.BillTypes
{
    public record ReadAllBillTypesQuery
        : IRequest<IEnumerable<ReadBillTypeDTO>>;
    public record ReadBillTypesByBillerIdQuery(Guid BillerId)
        : IRequest<IEnumerable<ReadBillTypeDTO>>;
    public class ReadAllBillTypesQueryHandler(IBillTypeRepository billTypeRepository)
        : IRequestHandler<ReadAllBillTypesQuery, IEnumerable<ReadBillTypeDTO>>
    {
        public async Task<IEnumerable<ReadBillTypeDTO>> 
            Handle(ReadAllBillTypesQuery request, CancellationToken cancellationToken)
        {
            var result = await billTypeRepository.ReadAllBillTypesAsync();
            return result.Select(billtype => new ReadBillTypeDTO
            (billtype.Id, billtype.BillerId, billtype.Name, billtype.Amount));
        }
    }
    public class ReadBillTypesByBillerIdQueryHandler(IBillTypeRepository billTypeRepository)
        : IRequestHandler<ReadBillTypesByBillerIdQuery, IEnumerable<ReadBillTypeDTO>>
    {
        public async Task<IEnumerable<ReadBillTypeDTO>> 
            Handle(ReadBillTypesByBillerIdQuery request, CancellationToken cancellationToken)
        {
            var result = await billTypeRepository.ReadBillTypesByBillerIdAsync(request.BillerId);
            return result.Select(billtype => new ReadBillTypeDTO
            (billtype.Id, billtype.BillerId, billtype.Name, billtype.Amount));
        }
    }
}
