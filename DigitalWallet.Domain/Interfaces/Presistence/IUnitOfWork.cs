using DigitalWallet.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using System.Data;
namespace DigitalWallet.Domain.Interfaces.Presistence
{
    //public interface IUnitOfWork : IDisposable
    //{
    //    void BeginTransaction();
    //    void Commit();
    //    void Rollback();

    //    // This gives the service access to the repository 
    //    // that is "hooked into" the current transaction
    //    IWalletRepository Wallets { get; }
    //}

    /* the Open/Close Principle version of the IUnitOfWork interface:*/
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        // A generic method to get any repository
        TRepository Repository<TRepository>() where TRepository : class;
    }
}
