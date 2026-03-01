    using System;
using System.Collections.Generic;
using System.Text;
using DigitalWallet.Domain.Exceptions.Abstract;
namespace DigitalWallet.Domain.Exceptions;

public sealed class RequiredFieldMissingException : AppException
{
    public string? EntityName { get; }

    public RequiredFieldMissingException(string? entityName)
        : base($"{entityName} is required.")
    {
        EntityName = entityName;
    }

    public override string ErrorCode => "REQUIRED_FIELD";
}
