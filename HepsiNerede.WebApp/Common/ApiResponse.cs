namespace HepsiNerede.WebApp.Common
{
    public class ApiResponse<T>
    {
        public string? Message { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; }

        public ApiResponse(T data, bool success = true, string? message = null)
        {
            this.Message = message;
            this.Success = success;
            this.Data = data;
        }
    }
}
