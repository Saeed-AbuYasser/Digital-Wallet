using DigitalWallet.Domain.Exceptions.Abstract;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DigitalWallet.Domain.Exceptions
{
    //public class NotFoundException<T>:AppException
    //{
    //    public string TableName { get; }
    //    public string ColumnName { get; }
    //    public T NotFoundValue { get; }
    //    public NotFoundException([NotNull] string tableName, [NotNull] string columnName,[NotNull] T notFoundValue)
    //        : base($"Couldn't find entity with field \'{columnName}\' = ({notFoundValue}) of type {typeof(T).Name} in table \'{tableName}\'.")
    //    {
    //        TableName = tableName;
    //        ColumnName = columnName;
    //        NotFoundValue = notFoundValue;
    //    }
    //    public override string ErrorCode => "NOT_FOUND";
    //}
    //public class NotFoundException : AppException
    //{
    //    public string TableName { get; }
    //    public NotFoundException([NotNull] string tableName)
    //        : base($"Couldn't find the entity in table \'{tableName}\'.")
    //    {
    //        TableName = tableName;
    //    }

    //    public override string ErrorCode => "NOT_FOUND";
    //}

    public class NotFoundException : AppException
    {
        public string EntityName { get; } = null!;
        public NotFoundException([NotNull] string message)
            : base(message)
        {
            EntityName = message;
        }
        public override string ErrorCode => "NOT_FOUND";
    }
}
