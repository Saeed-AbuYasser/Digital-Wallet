using DigitalWallet.Domain.Interfaces.Presistence;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Infrastructure.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace DigitalWallet.Infrastructure.Data;

/* * The Unit of Work pattern is a design pattern that maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems.*/

/// <summary>
/// Implements the Unit of Work pattern to coordinate database operations and manage repositories within a single
/// transactional context.
/// </summary>
/// <remarks>The UnitOfWork class provides a central point for managing multiple repository instances and
/// ensures that all changes are committed or rolled back as a single transaction. This helps maintain data
/// consistency and simplifies resource management. The class is not thread-safe and is intended for use within a
/// single logical operation or request.</remarks>
public class UnitOfWork : IUnitOfWork
{
    readonly DbSession session;
    // This stores our "active" repositories so we don't create them twice
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(IServiceProvider serviceProvider, DbSession _session)
    {
        _serviceProvider = serviceProvider;
        session = _session;

    }

    public TRepository Repository<TRepository>() where TRepository : class
    {
        // This asks the DI container: "Hey, who is registered for this interface?"
        var repository = _serviceProvider.GetService<TRepository>();

        if (repository == null)
        {
            throw new Exception($"Repository {typeof(TRepository).Name} is not registered in DI.");
        }

        return repository;
    }

    public void BeginTransaction()
    {
        session.Transaction = session.Connection.BeginTransaction();
    }

    public void Commit()
    {
        session.Transaction?.Commit();
        DisposeTransaction();
    }

    public void Rollback()
    {
        session.Transaction?.Rollback();
        DisposeTransaction();
    }

    private void DisposeTransaction()
    {
        session.Transaction?.Dispose();
        session.Transaction = null;
    }

}
