using System.Collections.Generic;

namespace Vegas.AspNetCore.Common.Models
{
    public class ApiResponse : ApiResponse<object>
    { }

    public class ApiResponse<TPayload> where TPayload : class
    {
        public bool IsSuccess { get; set; }

        public List<string> Messages { get; set; } = new List<string>();

        public TPayload MainResponse { get; set; }
    }
}
