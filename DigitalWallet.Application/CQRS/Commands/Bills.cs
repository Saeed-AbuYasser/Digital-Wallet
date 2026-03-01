using DigitalWallet.Application.DTOs.Bills;
using DigitalWallet.Domain.Interfaces.Services;
using DigitalWallet.Domain.Entities;
using MediatR;

namespace DigitalWallet.Application.CQRS.Commands.Bills
{
    public record IssueBillCommand(CreateBillDTO createBillDTO) : IRequest<ReadBillDTO>;
    public record PayBillCommand(Guid BillId) : IRequest;
    public class IssueBillCommandHandler(IBillIssuingService billIssuingService) : IRequestHandler<IssueBillCommand, ReadBillDTO>
    {
        public async Task<ReadBillDTO> Handle(IssueBillCommand request, CancellationToken cancellationToken)
        {
            var NewBill = await billIssuingService.IssueAsync(new BillEntity(Guid.Empty, request.createBillDTO.WalletId, request.createBillDTO.BillTypeId));
            return new ReadBillDTO(NewBill.Id, NewBill.WalletId, NewBill.WalletHolder!, NewBill.BillTypeId, NewBill.BillTypeName!, (decimal)NewBill.Amount!, (DateTime)NewBill.CreatedAt!, false);
        }
    }
    public class PayBillCommandHandler(IPayBillService payBillService) : IRequestHandler<PayBillCommand>
    {
        public async Task Handle(PayBillCommand request, CancellationToken cancellationToken)
        {
            await payBillService.PayBillAsync(request.BillId);
        }
    }
}
