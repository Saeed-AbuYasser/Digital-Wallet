using System;
using System.Collections.Generic;
using System.Text;

namespace DigitalWallet.Domain.Exceptions.Interfaces
{
    public interface IExceptionLogger
    {
        void Log(string message);
    }
}
