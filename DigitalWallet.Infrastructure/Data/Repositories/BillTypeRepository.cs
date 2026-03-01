using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Infrastructure.Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DigitalWallet.Infrastructure.Data.Repositories
{
    public class BillTypeRepository(DbSession session) : IBillTypeRepository
    {
        public async Task<BillTypeEntity> CreateBillTypeAsync(BillTypeEntity billTypeEntity)
        {
            if (billTypeEntity.Amount < 1) throw new ArgumentException("Bill types don't accept negative amounts!");
            const string sql = "INSERT INTO BillTypes (Id, BillerId, Name, Amount) VALUES (@Id, @BillerID, @Name, @Amount);";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    billTypeEntity = billTypeEntity with { Id = Guid.NewGuid() };
                    command.Parameters.AddWithValue("@Id", billTypeEntity.Id);
                    command.Parameters.AddWithValue("@Name", billTypeEntity.Name);
                    command.Parameters.AddWithValue("@Amount", billTypeEntity.Amount);
                    command.Parameters.AddWithValue("@BillerId", billTypeEntity.BillerId);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill Type");
            }
            return billTypeEntity;
        }
        public async Task<IEnumerable<BillTypeEntity>> ReadAllBillTypesAsync()
        {
            List<BillTypeEntity> result = new();
            const string sql = "Select Id, BillerId, Name, Amount From BillTypes;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            result.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetString(2), reader.GetDecimal(3)));
                        }
                    }
                    if (result.Count < 1) throw new NotFoundException("No bill types were found.");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill Type");
            }

            return result;
        }
        public async Task<IEnumerable<BillTypeEntity>> ReadBillTypesByBillerIdAsync(Guid BillerId)
        {
            List<BillTypeEntity> result = new();
            const string sql = "Select Id, BillerId, Name, Amount From BillTypes WHERE BillerId = @BillerId;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@BillerId", BillerId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            result.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetString(2), reader.GetDecimal(3)));
                        }
                    }
                    if (result.Count < 1) throw new NotFoundException("No bill types were found for this biller.");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Biller's Bill Types");
            }

            return result;
        }
        public async Task<BillTypeEntity> ReadBillTypeByIdAsync(Guid Id)
        {
            BillTypeEntity? result = null;
            const string sql = "Select Id, BillerId, Name, Amount From BillTypes WHERE Id = @Id;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            result = new(reader.GetGuid(0), reader.GetGuid(1), reader.GetString(2), reader.GetDecimal(3));
                        }
                    }
                    if (result == null) throw new NotFoundException($"bill type [{Id}] not found.");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill Type");
            }
            return result;
        }
        public async Task<BillTypeEntity> UpdateBillTypeAsync(BillTypeEntity billTypeEntity)
        {
            if (billTypeEntity.Amount < 1) throw new ArgumentException("Bill types don't accept negative amounts!"); 
            const string sql = "Update BillTypes Set Name = @Name, Amount = @Amount WHERE Id = @Id;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@Id", billTypeEntity.Id);
                    command.Parameters.AddWithValue("@Name", billTypeEntity.Name);
                    command.Parameters.AddWithValue("@Amount", billTypeEntity.Amount);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected < 1)
                    {
                        throw new NotFoundException($"bill type[{billTypeEntity.Id}] not found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill Type");
            }
            return billTypeEntity;
        }
        public async Task<decimal> GetBillTypeAmountByIdAsync(Guid Id)
        {
            const string sql = "Select Amount From BillTypes WHERE Id = @Id;";
            decimal amount = 0;
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    var result = await command.ExecuteScalarAsync();
                    if( result is null)
                    {
                        throw new NotFoundException($"Couldn't find bill type with Id [{Id}]");
                    }
                    amount = (decimal)result;
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill Type");
            }

            return amount;
        }
        public async Task DeleteBillTypeAsync(Guid Id)
        {
            const string sql = "Delete from BillTypes WHERE Id = @Id;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected < 1)
                    {
                        throw new NotFoundException($"bill type[{Id}] not found.");
                    }
                }
            }
            catch(SqlException ex) 
            {
                SqlExceptionMapper.MapSqlException(ex, "Bill Type");
            }
        }
    }
}
