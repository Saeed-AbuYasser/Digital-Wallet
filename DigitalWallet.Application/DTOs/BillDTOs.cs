using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Application.DTOs.Bills
{
    public record ReadBillDTO(Guid Id, Guid WalletId, string WalletHolder, Guid BillTypeId, string BillTypeName, decimal Amount, DateTime CreatedAt, bool IsPaid);
    public record CreateBillDTO(Guid WalletId, Guid BillTypeId);
}
