using DigitalWallet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Interfaces.Repositories
{
    public interface IBillerRepository
    {
        public Task<BillerEntity> CreateBillerAsync(BillerEntity billerEntity);
        public Task<IEnumerable<BillerEntity>> ReadAllBillersAsync();

    }
}
