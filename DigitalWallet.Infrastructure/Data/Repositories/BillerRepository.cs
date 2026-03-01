using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Interfaces.Repositories;
using DigitalWallet.Infrastructure.Helper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Infrastructure.Data.Repositories
{
    public class BillerRepository(DbSession session) : IBillerRepository
    {
        public async Task<BillerEntity> CreateBillerAsync(BillerEntity billerEntity)
        {
            const string sql = "INSERT INTO Billers (Id, WalletId, Name) VAlUES (@Id, @WalletId, @Name)";
            using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
            {
                billerEntity = billerEntity with { Id = Guid.NewGuid() };
                command.Parameters.AddWithValue("@Id", billerEntity.Id);
                command.Parameters.AddWithValue("@WalletId", billerEntity.WalletId);
                command.Parameters.AddWithValue("@Name", billerEntity.Name);
                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    throw SqlExceptionMapper.MapSqlException(ex, "Biller");
                }
            }
            return billerEntity;
        }


        public async Task<IEnumerable<BillerEntity>> ReadAllBillersAsync()
        {
            List<BillerEntity> result = new();
            const string sql = "Select Id, WalletId, Name From Billers;";
            try
            {
                using (var command = new SqlCommand(sql, session.Connection, session.Transaction))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            result.Add(new(reader.GetGuid(0), reader.GetGuid(1), reader.GetString(2)));
                        }
                    }
                    if (result == null || result.Count < 1) throw new NotFoundException("No Billers were found.");
                }
            }
            catch (SqlException ex)
            {
                throw SqlExceptionMapper.MapSqlException(ex, "Biller");
            }

            return result;

        }
    }
}
