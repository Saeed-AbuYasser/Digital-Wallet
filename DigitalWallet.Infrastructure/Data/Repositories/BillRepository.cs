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
    public class BillRepository(DbSession session) : IBillRepository
    {
        public async Task<IEnumerable<BillEntity>> ReadAllBillsAsync()
        {
            List<BillEntity> bills = new List<BillEntity>();
            const string sql = @"
            Select B.Id, B.WalletId, B.BillTypeId, B.Amount, B.CreatedAt, B.PaidAt, W.Holder, BT.Name 
            FROM Bills B JOIN BillTypes BT ON B.BillTypeId = BT.Id 
            JOIN Wallets W ON B.WalletId = W.Id;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetGuid(2), reader.GetDecimal(3), reader.GetDateTime(4), reader[5] == DBNull.Value ? null : reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
                        }

                    }
                    if (bills.Count < 1)
                    {
                        throw new NotFoundException("No bills were found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill");
            }

            return bills;
        }
        public async Task<IEnumerable<BillEntity>> ReadBillsByWalletIdAsync(Guid WalletId)
        {
            List<BillEntity> bills = new List<BillEntity>();
            const string sql = @"
            Select B.Id, B.WalletId, B.BillTypeId, B.Amount, B.CreatedAt, B.PaidAt, W.Holder, BT.Name 
            FROM Bills B JOIN BillTypes BT ON B.BillTypeId = BT.Id 
            JOIN Wallets W ON B.WalletId = W.Id
            WHERE B.WalletId = @WalletId;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@WalletId", WalletId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetGuid(2), reader.GetDecimal(3), reader.GetDateTime(4), reader[5] == DBNull.Value ? null : reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
                        }

                    }
                    if (bills.Count < 1)
                    {
                        throw new NotFoundException("No bills were found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill");
            }
            return bills;
        }
        public async Task<IEnumerable<BillEntity>> ReadPaidBillsByWalletIdAsync(Guid WalletId)
        {
            List<BillEntity> bills = new List<BillEntity>();
            const string sql = @"
            Select B.Id, B.WalletId, B.BillTypeId, B.Amount, B.CreatedAt, B.PaidAt, W.Holder, BT.Name 
            FROM Bills B JOIN BillTypes BT ON B.BillTypeId = BT.Id 
            JOIN Wallets W ON B.WalletId = W.Id 
            WHERE B.WalletId = @WalletId AND B.PaidAt is not null;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@WalletId", WalletId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetGuid(2), reader.GetDecimal(3), reader.GetDateTime(4), reader[5] == DBNull.Value ? null : reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
                        }

                    }
                    if (bills.Count < 1)
                    {
                        throw new NotFoundException("No bills were found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill");
            }
            return bills;
        }
        public async Task<IEnumerable<BillEntity>> ReadUnPaidBillsByWalletIdAsync(Guid WalletId)
        {
            List<BillEntity> bills = new List<BillEntity>();
            const string sql = @"
            Select B.Id, B.WalletId, B.BillTypeId, B.Amount, B.CreatedAt, B.PaidAt, W.Holder, BT.Name 
            FROM Bills B JOIN BillTypes BT ON B.BillTypeId = BT.Id 
            JOIN Wallets W ON B.WalletId = W.Id
            WHERE B.WalletId = @WalletId AND B.PaidAt is null;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@WalletId", WalletId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            bills.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetGuid(2), reader.GetDecimal(3), reader.GetDateTime(4), reader[5] == DBNull.Value ? null : reader.GetDateTime(5), reader.GetString(6), reader.GetString(7)));
                        }

                    }
                    if (bills.Count < 1)
                    {
                        throw new NotFoundException("No bills were found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill");
            }
            return bills;
        }
        public async Task<BillEntity> ReadBillByIdAsync(Guid Id)
        {
            BillEntity? bills = null;
            const string sql = @"
            Select B.Id, B.WalletId, B.BillTypeId, B.Amount, B.CreatedAt, B.PaidAt, W.Holder, BT.Name 
            FROM Bills B JOIN BillTypes BT ON B.BillTypeId = BT.Id 
            JOIN Wallets W ON B.WalletId = W.Id
            WHERE B.Id = @Id;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            bills = new(reader.GetGuid(0), reader.GetGuid(1), reader.GetGuid(2), reader.GetDecimal(3), reader.GetDateTime(4), reader[5] == DBNull.Value? null: reader.GetDateTime(5), reader.GetString(6), reader.GetString(7));
                        }

                    }
                    if (bills == null)
                    {
                        throw new NotFoundException($"bill [{Id}] not found.");
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill");
            }
            return bills;
        }
        public async Task<BillEntity> MarkBillAsPaidAsync(BillEntity billEntity)
        {
            if (billEntity.PaidAt != null) throw new Exception($"Bill [{billEntity.Id}] can't be updated.");
            const string sql = "Update Bills SET PaidAt = @PaidAt WHERE Id = @Id;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@Id", billEntity.Id);
                    billEntity = billEntity with { PaidAt = DateTime.Now };
                    command.Parameters.AddWithValue("@PaidAt", billEntity.PaidAt);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected < 1) throw new NotFoundException($"Bill [{billEntity.Id}] not found.");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill");
            }
            billEntity = await ReadBillByIdAsync(billEntity.Id);
            return billEntity;
        }
        public async Task<BillEntity> CreateBillAsync(BillEntity billEntity)
        {
            const string sql = "INSERT INTO Bills (Id, WalletId, BillTypeId, Amount, CreatedAt, PaidAt) VALUES (@Id, @WalletId, @BillTypeId, @Amount, @CreatedAt, @PaidAt);";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    billEntity = billEntity with { Id = Guid.NewGuid() };
                    command.Parameters.AddWithValue("@Id", billEntity.Id);
                    command.Parameters.AddWithValue("@WalletId", billEntity.WalletId);
                    command.Parameters.AddWithValue("@BillTypeId", billEntity.BillTypeId);
                    command.Parameters.AddWithValue("@Amount", billEntity.Amount);
                    billEntity = billEntity with { CreatedAt = DateTime.Now };
                    command.Parameters.AddWithValue("@CreatedAt", billEntity.CreatedAt);
                    command.Parameters.AddWithValue("@PaidAt", DBNull.Value);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                }

            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Bill");
            }
            billEntity = await ReadBillByIdAsync(billEntity.Id);
            return billEntity;
        }
    
    }
}
