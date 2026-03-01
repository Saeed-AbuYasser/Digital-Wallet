using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Exceptions.Abstract
{
    public abstract class AppException : Exception
    {
        protected AppException(string message) : base(message) { }

        public abstract string ErrorCode { get; }
    }
}
