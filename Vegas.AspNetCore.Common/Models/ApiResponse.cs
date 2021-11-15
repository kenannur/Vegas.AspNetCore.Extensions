using System.Collections.Generic;

namespace Vegas.AspNetCore.Common.Models
{
    public class ApiResponse : ApiResponse<object>
    { }

    public class ApiResponse<TResponse> where TResponse : class
    {
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public TResponse MainResponse { get; set; }

        public static ApiResponse<TResponse> Success(TResponse response) => new()
        {
            IsSuccess = true,
            MainResponse = response
        };
    }
}
