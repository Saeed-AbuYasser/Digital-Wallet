using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Entities
{
    public record BillTypeEntity(Guid Id,Guid BillerId, string Name, decimal Amount);
}
