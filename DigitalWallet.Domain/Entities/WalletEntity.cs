using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Entities
{
    
    public record WalletEntity(Guid Id, string Holder, decimal Balance);
}
