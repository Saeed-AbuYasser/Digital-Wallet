using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Infrastructure.Data
{
    public class DbSession : IDisposable
    {
        public SqlConnection Connection { get; }
        public SqlTransaction? Transaction { get; set; } // Can be null if no transaction is active

        public DbSession(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public void Dispose()
        {
            Transaction?.Dispose();
            Connection?.Dispose();
        }
    }
}
