namespace Printstream.Models
{
    public class ApiResponse
    {
        public string? SessionID { get; private set; }
        public string? Message { get; private set; }
        public string? ErrorCode { get; private set; }

        public static ApiResponse Success(string SessionID, string Message)
        {
            return new ApiResponse
            {
                SessionID = SessionID,
                Message = Message,
                ErrorCode = null
            };
        }

        public static ApiResponse Error(string ErrorCode)
        {
            return new ApiResponse
            {
                Message = null,
                ErrorCode = ErrorCode
            };
        }
    }
}
