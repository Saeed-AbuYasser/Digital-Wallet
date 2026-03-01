using DigitalWallet.Domain.Exceptions.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Exceptions
{
    public sealed class ForeignKeyViolationException : AppException
    {
        public string? EntityName { get; }

        public ForeignKeyViolationException(string? entityName)
            : base($"Invalid reference for {entityName}.")
        {
            EntityName = entityName;
        }

        public override string ErrorCode => "FOREIGN_KEY_VIOLATION";
    }
}
