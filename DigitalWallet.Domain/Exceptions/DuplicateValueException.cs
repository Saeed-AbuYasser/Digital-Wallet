using DigitalWallet.Domain.Exceptions.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Exceptions
{
    public sealed class DuplicateValueException : AppException
    {
        public string EntityName { get; }

        public DuplicateValueException(string entityName)
            : base($"Duplicate value for \'{entityName}\'.")
        {
            EntityName = entityName;
        }

        public override string ErrorCode => "DUPLICATE_VALUE";
    }
}
