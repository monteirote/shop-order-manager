namespace OrderManager.Models
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; } = default!;

        public static ServiceResponse<T> Ok (T? data = default, string message = "")
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ServiceResponse<T> Error (string message)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default!
            };
        }
    }
}
