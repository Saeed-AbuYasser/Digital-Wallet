using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DigitalWallet.Application.DTOs.Wallets
{
    public record ReadWalletDTO(
        Guid Id,
        string Name,
        decimal Balance
    );
    public record CreateWalletDTO(
        [Required]
        string Name,
        [Required]
        decimal InitialBalance
    );
}
