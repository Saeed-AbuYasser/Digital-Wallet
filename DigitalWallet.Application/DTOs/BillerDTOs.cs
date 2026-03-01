using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Application.DTOs.Billers
{
    public record CreateBillerDTO(Guid WalletId, string Name);
    public record ReadBillerDTO(Guid Id, Guid WalletId, string Name);
}
