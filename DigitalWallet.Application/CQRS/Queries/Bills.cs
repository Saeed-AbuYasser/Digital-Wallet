using DigitalWallet.Application.DTOs.Bills;
using DigitalWallet.Domain.Interfaces.Repositories;
using MediatR;
namespace DigitalWallet.Application.CQRS.Queries.Bills
{
    public record ReadAllBillsQuery : IRequest<IEnumerable<ReadBillDTO>>;
    public class ReadAllBillsQueryHandler(IBillRepository billRepository) : IRequestHandler<ReadAllBillsQuery, IEnumerable<ReadBillDTO>>
    {
        public async Task<IEnumerable<ReadBillDTO>> Handle(ReadAllBillsQuery request, CancellationToken cancellationToken)
        {
            var billEntities = await billRepository.ReadAllBillsAsync();
            return billEntities.Select(bill => new ReadBillDTO(
                bill.Id,
                bill.WalletId,
                bill.WalletHolder!,
                bill.BillTypeId,
                bill.BillTypeName!,
                (decimal)bill.Amount!,
                (DateTime)bill.CreatedAt!,
                bill.PaidAt != null
            ));
        }
    }
    public record ReadPaidBillsByWalletIdQuery(Guid WalletId) : IRequest<IEnumerable<ReadBillDTO>>;
    public class ReadPaidBillsByWalletIdQueryHandler(IBillRepository billRepository) : IRequestHandler<ReadPaidBillsByWalletIdQuery, IEnumerable<ReadBillDTO>>
    {
        public async Task<IEnumerable<ReadBillDTO>> Handle(ReadPaidBillsByWalletIdQuery request, CancellationToken cancellationToken)
        {
            var billEntities = await billRepository.ReadPaidBillsByWalletIdAsync(request.WalletId);
            return billEntities.Select(bill => new ReadBillDTO(
                bill.Id,
                bill.WalletId,
                bill.WalletHolder!,
                bill.BillTypeId,
                bill.BillTypeName!,
                (decimal)bill.Amount!,
                (DateTime)bill.CreatedAt!,
                bill.PaidAt != null
            ));
        }
    }
    public record ReadUnPaidBillsByWalletIdQuery(Guid WalletId) : IRequest<IEnumerable<ReadBillDTO>>;
    public class ReadUnPaidBillsByWalletIdQueryHandler(IBillRepository billRepository) : IRequestHandler<ReadUnPaidBillsByWalletIdQuery, IEnumerable<ReadBillDTO>>
    {
        public async Task<IEnumerable<ReadBillDTO>> Handle(ReadUnPaidBillsByWalletIdQuery request, CancellationToken cancellationToken)
        {
            var billEntities = await billRepository.ReadUnPaidBillsByWalletIdAsync(request.WalletId);
            return billEntities.Select(bill => new ReadBillDTO(
                bill.Id,
                bill.WalletId,
                bill.WalletHolder!,
                bill.BillTypeId,
                bill.BillTypeName!,
                (decimal)bill.Amount!,
                (DateTime)bill.CreatedAt!,
                bill.PaidAt != null
            ));
        }
    }
    public record ReadBillByIdQuery(Guid Id) : IRequest<ReadBillDTO>;
    public class ReadBillByIdQueryHandler(IBillRepository billRepository) : IRequestHandler<ReadBillByIdQuery, ReadBillDTO>
    {
        public async Task<ReadBillDTO> Handle(ReadBillByIdQuery request, CancellationToken cancellationToken)
        {
            var billEntity = await billRepository.ReadBillByIdAsync(request.Id);
            return new ReadBillDTO(
                billEntity.Id,
                billEntity.WalletId,
                billEntity.WalletHolder!,
                billEntity.BillTypeId,
                billEntity.BillTypeName!,
                (decimal)billEntity.Amount!,
                (DateTime)billEntity.CreatedAt!,
                billEntity.PaidAt != null
            );
        }
    }
}
