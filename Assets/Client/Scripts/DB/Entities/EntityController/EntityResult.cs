using System;
using Client.Scripts.DB.Entities.Base;

namespace Client.Scripts.DB.Entities.EntityController
{
    internal sealed class EntityResult<TData> where TData : class
    {
        internal bool IsSuccess { get; private set; }
        internal EntryData<TData> EntryData { get; private set; }
        internal string ErrorMessage { get; private set; }
        internal Exception Exception { get; private set; }

        internal static EntityResult<TData> Success(EntryData<TData> data) =>
            new()
            {
                IsSuccess = true,
                EntryData = data
            };

        internal static EntityResult<TData> Failure(string message, Exception ex = null) =>
            new()
            {
                IsSuccess = false,
                ErrorMessage = message,
                Exception = ex
            };
    }
}