using System.Collections.Generic;

namespace Vegas.AspNetCore.Common.Models
{
    public class ApiResponse
    {
        public bool IsSuccess { get; set; }

        public List<string> Messages { get; set; } = new List<string>();

        public object MainResponse { get; set; }
    }
}
