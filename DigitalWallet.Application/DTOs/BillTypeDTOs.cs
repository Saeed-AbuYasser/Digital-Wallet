using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Application.DTOs.BillTypes
{
    public record ReadBillTypeDTO(Guid Id, Guid BillerType, string Name, decimal Amount);
    public record CreateBillTypeDTO(Guid BillerType,string Name, decimal Amount);
    public record UpdateBillTypeDTO(Guid Id, string Name, decimal Amount);
}
