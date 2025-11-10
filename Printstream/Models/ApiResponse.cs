namespace Printstream.Models
{
    public class ApiResponse<T>
    {
        public T? Data { get; private set; }
        public string? Message { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static ApiResponse<T> Success(T Data, string message)
        {
            return new ApiResponse<T>
            {
                Data = Data,
                Message = message,
                ErrorMessage = null
            };
        }

        public static ApiResponse<T> Error(string error)
        {
            return new ApiResponse<T>
            {                
                Message = null,
                ErrorMessage = error
            };
        }
    }
}