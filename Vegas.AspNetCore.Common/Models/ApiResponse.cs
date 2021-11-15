using System.Collections.Generic;
using System.Linq;

namespace Vegas.AspNetCore.Common.Models
{
    public class ApiResponse : ApiResponse<object>
    { }

    public class ApiResponse<TResponse> where TResponse : class
    {
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public TResponse MainResponse { get; set; }

        public static ApiResponse<TResponse> Success(TResponse response) => new ApiResponse<TResponse>
        {
            IsSuccess = true,
            MainResponse = response
        };

        public static ApiResponse<TResponse> Failure(IEnumerable<string> messages) => new ApiResponse<TResponse>
        {
            IsSuccess = false,
            Messages = messages.ToList()
        };
    }
}
