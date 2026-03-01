using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Infrastructure.Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DigitalWallet.Infrastructure.Data.Repositories
{
    public class TransferRepository(DbSession session) : ITransferRepository
    {
        public async Task<TransferEntity> CreateTransferAsync(TransferEntity transferEntity)
        {
            if (transferEntity.Amount < 1) throw new DataCorrectnessViolationException("Transfer");
            const string sql = "INSERT INTO Transfers (Id, SourceWalletId, DestinationWalletId, Amount, Date) VALUES (@Id, @SourceWalletId, @DestinationWalletId, @Amount, @Date);";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    transferEntity = transferEntity with { Id = Guid.NewGuid() };
                    command.Parameters.AddWithValue("@Id", transferEntity.Id);
                    command.Parameters.AddWithValue("@SourceWalletId", transferEntity.SourceWalletId);
                    command.Parameters.AddWithValue("@DestinationWalletId", transferEntity.DestinationWalletId);
                    command.Parameters.AddWithValue("@Amount", transferEntity.Amount);
                    transferEntity = transferEntity with { Date = DateTime.Now };
                    command.Parameters.AddWithValue("@Date", transferEntity.Date);
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    if (rowsAffected < 1) throw new Exception("Unkown error has occurred.");
                }
            }

            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Transfer");
            }
            return transferEntity;
        }
        public async Task<IEnumerable<TransferEntity>> ReadAllTransfersAsync()
        {
            const string sql = "SELECT Id, SourceWalletId, DestinationWalletId, Amount, Date FROM Transfers;";
            var transfers = new List<TransferEntity>();
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var transfer = new TransferEntity
                            (
                                reader.GetGuid(0),
                                reader.GetGuid(1),
                                reader.GetGuid(2),
                                reader.GetDecimal(3),
                                reader.GetDateTime(4)
                            );
                            transfers.Add(transfer);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Transfer");
            }
            return transfers;
        }
        public async Task<IEnumerable<TransferEntity>> ReadTransfersBySourceWalletIdAsync(Guid SourceWalletId)
        {
            const string sql = "SELECT Id, SourceWalletId, DestinationWalletId, Amount, Date FROM Transfers WHERE SourceWalletId = @SourceWalletId;";
            var transfers = new List<TransferEntity>();
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@SourceWalletId", SourceWalletId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var transfer = new TransferEntity
                            (
                                reader.GetGuid(0),
                                reader.GetGuid(1),
                                reader.GetGuid(2),
                                reader.GetDecimal(3),
                                reader.GetDateTime(4)
                            );
                            transfers.Add(transfer);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Transfer");
            }
            return transfers;
        }
        public async Task<IEnumerable<TransferEntity>> ReadTransfersByDestinationWalletIdAsync(Guid DestinationWalletId)
        {
            const string sql = "SELECT Id, SourceWalletId, DestinationWalletId, Amount, Date FROM Transfers WHERE DestinationWalletId = @DestinationWalletId;";
            var transfers = new List<TransferEntity>();
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@DestinationWalletId", DestinationWalletId);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var transfer = new TransferEntity
                            (
                                reader.GetGuid(0),
                                reader.GetGuid(1),
                                reader.GetGuid(2),
                                reader.GetDecimal(3),
                                reader.GetDateTime(4)
                            );
                            transfers.Add(transfer);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Transfer");
            }
            return transfers;
        }
    }
}
