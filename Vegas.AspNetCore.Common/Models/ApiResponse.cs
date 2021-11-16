using System.Collections.Generic;
using System.Linq;

namespace Vegas.AspNetCore.Common.Models
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public object MainResponse { get; set; }

        public static ApiResponse Success(object response) => new ApiResponse
        {
            IsSuccess = true,
            MainResponse = response
        };

        public static ApiResponse Failure(IEnumerable<string> messages) => new ApiResponse
        {
            IsSuccess = false,
            Messages = messages.ToList()
        };
    }
}
