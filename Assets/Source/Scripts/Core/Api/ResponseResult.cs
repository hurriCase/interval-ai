namespace Source.Scripts.Core.Api
{
    internal readonly struct ResponseResult<T> where T : class
    {
        public T Data { get; }
        public bool Success { get; }

        public ResponseResult(T data, bool success)
        {
            Data = data;
            Success = success;
        }

        public ResponseResult(bool success)
        {
            Data = null;
            Success = success;
        }
    }
}