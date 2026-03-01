using DigitalWallet.Domain.Exceptions.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Exceptions
{
    public class DataCorrectnessViolationException : AppException
    {
        public string EntityName { get; }
        public DataCorrectnessViolationException(string entityName) :
            base($"{entityName} value isn't correct.")
        {
            EntityName = entityName;
        }
        public override string ErrorCode => "DATA_VALUE_NOT_CORRECT";
    }
}
