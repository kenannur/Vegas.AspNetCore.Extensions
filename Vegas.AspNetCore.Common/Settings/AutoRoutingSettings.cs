using System.Collections.Generic;

namespace Vegas.AspNetCore.Common.Settings
{
    public class AutoRoutingSettings : IAutoRoutingSettings
    {
        public List<AutoRouting> AutoRoutings { get; set; }
    }

    public interface IAutoRoutingSettings
    {
        List<AutoRouting> AutoRoutings { get; set; }
    }

    public class AutoRouting
    {
        // v1/Auth/SignUp
        public string FromRequestUri { get; set; }

        // GET - POST - PUT - DELETE
        public string HttpMethod { get; set; }

        // http://localhost:7001
        public string ToBaseAddress { get; set; }

        // v1/Auth/SignUp
        public string ToRequestUri { get; set; }

        // http://localhost:7001/v1/Auth/SignUp?abc=123
        public string FullAddress(string queryString = "")
        {
            return $"{ToBaseAddress}/{ToRequestUri}{queryString}";
        }
    }
}
