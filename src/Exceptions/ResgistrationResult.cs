using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentActivities.src.Exceptions
{
    public class ResgistrationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public string? Error { get; private set; }

        private ResgistrationResult(bool isSuccess, T? data, string? error)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
        }

        public static ResgistrationResult<T> Success(T data)
        {
            return new ResgistrationResult<T>(true, data, null);
        }

        public static ResgistrationResult<T> Failure(string error)
        {
            return new ResgistrationResult<T>(false, default(T), error);
        }
    }
}
