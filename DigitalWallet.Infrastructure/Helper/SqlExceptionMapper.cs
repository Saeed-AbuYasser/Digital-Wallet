using DigitalWallet.Domain.Exceptions;
using DigitalWallet.Domain.Exceptions.Abstract;
using Microsoft.Data.SqlClient;
using System.Net;
using System.Text.RegularExpressions;

namespace DigitalWallet.Infrastructure.Helper
{
    public static class SqlExceptionMapper
    {
        //547 foreign key, check
        //2628 string longer than expected
        //8115 Arithmetic overflow (numeric longer than expected)
        //515 not null violation
        //206 operand type clash

        public static AppException MapSqlException(SqlException ex, string EntityName)
        {
            return ex.Number switch
            {
                2627 or 2601 =>   // Unique constraint
                    new DuplicateValueException(EntityName),
                547 when ex.Message.Contains("FOREIGN KEY", StringComparison.OrdinalIgnoreCase) =>            // Foreign key violation
                    new ForeignKeyViolationException(EntityName),
                547 when ex.Message.Contains("CHECK", StringComparison.OrdinalIgnoreCase) =>                  // Check constraint violation
                    new DataCorrectnessViolationException(EntityName),
                8115 =>           //Arithmetic overflow
                    new DataCorrectnessViolationException(EntityName),
                206 =>           //Arithmetic overflow
                    new DataCorrectnessViolationException(EntityName),
                515 =>            // Operand type clash
                    new RequiredFieldMissingException(EntityName),
                2628 =>           // String length is longer than expected.
                    new DataValueBiggerThanExpectedException(EntityName),

                _ =>
                    throw new Exception("Unkown error occurred") // unknown
            };
        }

        private static string? TryExtractFieldName(string message)
        {
            // Looks for constraint name between quotes
            var match = Regex.Match(message, @"constraint '([^']+)'", RegexOptions.IgnoreCase);
            if (!match.Success)
                return null;

            var constraintName = match.Groups[1].Value;

            // UQ_Table_Field → Field
            var parts = constraintName.Split('_');
            return parts.Length >= 3 ? parts[^1] : null;
        }
    }
}
