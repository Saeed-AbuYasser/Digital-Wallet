using DigitalWallet.Domain.Exceptions.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Exceptions
{
    public class DataValueBiggerThanExpectedException: AppException
    {
        public string? EntityName { get; }

        public DataValueBiggerThanExpectedException(string? entityName)
            : base($"{entityName} value is bigger than expected.")
        {
            EntityName = entityName;
        }

        public override string ErrorCode => "DATA_VALUE_TOO_LONG";
    }
}
