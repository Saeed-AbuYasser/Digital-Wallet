using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using DigitalWallet.Infrastructure.Helper;
namespace DigitalWallet.Infrastructure.Data.Repositories;

public class WalletRepository(DbSession session) : IWalletRepository
{
    public async Task<WalletEntity> CreateWalletAsync(WalletEntity walletEntity)
    {
        if(walletEntity.Balance < 0)
        {
            throw new DataCorrectnessViolationException("Wallet Balance");
        }
        const string sql = "INSERT INTO Wallets (Id, Holder, Balance) VAlUES (@Id, @Holder, @Balance)";
        using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
        {
            walletEntity = walletEntity with { Id = Guid.NewGuid() };
            command.Parameters.AddWithValue("@Id", walletEntity.Id);
            command.Parameters.AddWithValue("@Holder", walletEntity.Holder);
            command.Parameters.AddWithValue("@Balance", walletEntity.Balance);
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Wallet");
            }
        }
        return walletEntity;
    }
    public async Task<IEnumerable<WalletEntity>> ReadAllWalletsAsync()
    {
        List<WalletEntity> result = new();
        const string sql = "Select Id, Holder, Balance From Wallets;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(new(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(2)));
                    }
                }
                if (result == null || result.Count < 1) throw new NotFoundException("No wallets were found.");
            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Wallet");
        }

        return result;
    }
    public async Task<WalletEntity> ReadWalletByIdAsync(Guid id)
    {
        WalletEntity? result = null;

        const string sql = "Select Id, Holder, Balance From Wallets WHERE Id = @id;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result = new(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(2));
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Wallet");
        }
        if (result == null)
        {
            throw new NotFoundException($"Wallet [{id}] not found.");
        }
        return result;
    }

    public async Task<WalletEntity> ReadWalletByNameAsync(string Holder)
    {
        WalletEntity? result = null;

        const string sql = "Select Id, Holder, Balance From Wallets WHERE Holder = @Holder;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                command.Parameters.AddWithValue("@Holder", Holder);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result = new(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(2));
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Wallet");
        }
        if (result == null)
        {
            throw new NotFoundException($"Wallet with holder name: [{Holder}] not found.");
        }
        return result;
    }
    public async Task<bool> WalletExistsByIdAsync(Guid Id)
    {
        const string sql = "Select CASE WHEN EXISTS (SELECT 1 From Wallets WHERE Id = @id) THEN 1 ELSE 0 END;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                command.Parameters.AddWithValue("@id", Id);
                var scalarResult = await command.ExecuteScalarAsync();
                var result = scalarResult is not null ? Convert.ToInt32(scalarResult) : 0;
                return result == 1;
            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Wallet");
        }
    }
    public async Task<WalletEntity> UpdateWalletAsync(WalletEntity walletEntity)
    {
            const string sql = "UPDATE Wallets SET Balance = @balance WHERE Id = @id";
        using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
        {
            command.Parameters.AddWithValue("@balance", walletEntity.Balance);
            command.Parameters.AddWithValue("@id", walletEntity.Id);
            try
            {
                var rowsAffected = await command.ExecuteNonQueryAsync();
                if (rowsAffected == 0)
                {
                    throw new NotFoundException($"Wallet [{walletEntity.Id}] not found.");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex,"Wallet Balance Amount");
            }
        }
        return walletEntity;
    }
    public async Task<Guid> DeleteWalletByIdAsync(Guid Id)
    {
        const string sql = "Delete From Wallets WHERE Id = @id;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                command.Parameters.AddWithValue("@id", Id);
                var result = command.ExecuteNonQuery();
                if (result < 1)
                {
                    throw new NotFoundException($"Wallet [{Id}] not found.");
                }

            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Wallet");
        }
        return Id;
    }
    public async Task<WalletEntity> ReadBillerWalletByBillerIdAsync(Guid BillerId)
    {
        WalletEntity? result = null;

        const string sql = @"Select W.Id, W.Holder, W.Balance From Wallets W
        JOIN Billers B ON B.WalletId = W.Id 
        WHERE B.Id = @BillerId;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                command.Parameters.AddWithValue("@BillerId", BillerId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result = new(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(2));
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Biller");
        }
        if (result == null)
        {
            throw new NotFoundException($"Biller with ID:[{BillerId}] not found.");
        }
        return result;
    }

    public async Task<WalletEntity> ReadWalletByBillTypeIdAsync(Guid BillTypeId)
    {
        WalletEntity? result = null;

        const string sql = @"Select W.Id, W.Holder, W.Balance From Wallets W
        JOIN Billers B ON B.WalletId = W.Id 
        JOIN BillTypes BT ON BT.BillerId = B.Id
        WHERE BT.Id = @BillTypeId;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                command.Parameters.AddWithValue("@BillTypeId", BillTypeId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result = new(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(2));
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Bill Type");
        }
        if (result == null)
        {
            throw new NotFoundException($"Bill type with ID:[{BillTypeId}] not found.");
        }
        return result;
    }

    public async Task<WalletEntity> ReadWalletByFrequentBillTypeIdAsync(Guid FrequentBillTypeId)
    {
        WalletEntity? result = null;

        const string sql = @"Select W.Id, W.Holder, W.Balance From Wallets W
        JOIN Billers B ON B.WalletId = W.Id 
        JOIN FrequentBillTypes FBT ON FBT.BillerId = B.Id
        WHERE FBT.Id = @FrequentBillTypeId;";
        try
        {
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                command.Parameters.AddWithValue("@FrequentBillTypeId", FrequentBillTypeId);
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result = new(reader.GetGuid(0), reader.GetString(1), reader.GetDecimal(2));
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            throw SqlExceptionMapper.MapSqlException(ex, "Frequent Bill Type");
        }
        if (result == null)
        {
            throw new NotFoundException($"Frequent Bill type with ID:[{FrequentBillTypeId}] not found.");
        }
        return result;
    }
}
