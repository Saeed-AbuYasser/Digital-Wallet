using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Enums;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Infrastructure.Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Infrastructure.Data.Repositories
{
    public class TransactionRepository(DbSession session) : ITransactionRepository
    {
        public async Task<TransactionEntity> CreateTransactionAsync(TransactionEntity transactionEntity)
        {
            const string sql = "INSERT INTO Transactions (Id, WalletId, Amount, Type, CreatedAt) Values (@Id, @WalletId, @Amount, @Type, @CreatedAt);";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    transactionEntity = transactionEntity with { Id = Guid.NewGuid() };
                    command.Parameters.AddWithValue("@Id", transactionEntity.Id);
                    command.Parameters.AddWithValue("@WalletId", transactionEntity.WalletId);
                    command.Parameters.AddWithValue("@Amount", transactionEntity.Amount);
                    command.Parameters.AddWithValue("@Type", transactionEntity.Type.ToString());
                    transactionEntity = transactionEntity with { CreatedAt = DateTime.Now };
                    command.Parameters.AddWithValue("@CreatedAt", transactionEntity.CreatedAt);
                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Transaction");
            }
            return transactionEntity;
        }
        public async Task<IEnumerable<TransactionEntity>> ReadAllTransactionsAsync()
        {
            const string sql = "Select Id, WalletId, Amount, Type, CreatedAt From Transactions;";
            List<TransactionEntity> transactions = new();
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            TransactionTypeEnum type = (TransactionTypeEnum)Enum.Parse(typeof(TransactionTypeEnum), reader.GetString(3));
                            transactions.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetDecimal(2), type, reader.GetDateTime(4)));
                        }
                    }
                    if (transactions.Count < 1) throw new NotFoundException("No transactions were found.");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Transaction");
            }
            return transactions;
        }
        public async Task<IEnumerable<TransactionEntity>> ReadTransactionsByWalletIdAsync(Guid WalletId)
        {
            const string sql = "Select (Id, WalletId, Amount, Type, CreatedAt) From Transactions WHERE WalletId = @WalletId;";
            List<TransactionEntity> transactions = new();
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    command.Parameters.AddWithValue("@WalletId", WalletId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            TransactionTypeEnum type = (TransactionTypeEnum)Enum.Parse(typeof(TransactionTypeEnum), reader.GetString(0));
                            transactions.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetDecimal(2), type, reader.GetDateTime(4)));
                        }
                    }
                    if (transactions.Count < 1) throw new NotFoundException($"No transactions were found for wallet [{WalletId}].");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Transaction");
            }
            return transactions;
        }
    }
}
