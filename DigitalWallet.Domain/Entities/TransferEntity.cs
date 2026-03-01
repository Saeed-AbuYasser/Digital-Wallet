using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Entities
{
    public record TransferEntity(Guid Id, Guid SourceWalletId, Guid DestinationWalletId, decimal Amount, DateTime Date);
}
