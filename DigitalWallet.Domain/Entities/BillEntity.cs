using System;
using System.Collections.Generic;
using System.Text;
namespace DigitalWallet.Domain.Entities
{
    public record BillEntity(Guid Id, Guid WalletId,Guid BillTypeId, decimal? Amount = null, DateTime? CreatedAt = null, DateTime? PaidAt = null, string? WalletHolder = null, string? BillTypeName = null);
}
