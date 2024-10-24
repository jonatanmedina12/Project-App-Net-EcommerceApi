namespace API.Errors
{
    public class CustomException : Exception
    {
        public string ErrorCode { get; }
        public int StatusCode { get; }

        public CustomException(string message, string errorCode, int statusCode = 400)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
