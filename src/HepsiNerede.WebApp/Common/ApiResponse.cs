namespace HepsiNerede.WebApp.Common
{
    /// <summary>
    /// Represents a generic API response without specific data.
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// Gets or sets the optional message associated with the response.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether the operation was successful.</param>
        /// <param name="message">An optional message associated with the response.</param>
        public ApiResponse(bool success = true, string? message = null)
        {
            this.Message = message;
            this.Success = success;
        }
    }

    /// <summary>
    /// Represents a generic API response with specific data of type T.
    /// </summary>
    /// <typeparam name="T">The type of data associated with the response.</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Gets or sets the optional message associated with the response.
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// Gets or sets the data associated with the response.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data associated with the response.</param>
        /// <param name="success">A value indicating whether the operation was successful.</param>
        /// <param name="message">An optional message associated with the response.</param>
        public ApiResponse(T data, bool success = true, string? message = null)
        {
            this.Message = message;
            this.Success = success;
            this.Data = data;
        }
    }
}
