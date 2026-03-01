using System;
using System.Collections.Generic;
using System.Text;
using DigitalWallet.Domain.Enums;
namespace DigitalWallet.Domain.Entities
{
    
    public record TransactionEntity(Guid Id,Guid WalletId, decimal Amount, TransactionTypeEnum Type, DateTime CreatedAt);
}
