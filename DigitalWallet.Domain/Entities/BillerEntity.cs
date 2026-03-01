using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Entities
{
    public record BillerEntity(Guid Id, Guid WalletId, string Name);
}
