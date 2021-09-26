using System;
using System.Collections.Generic;
using System.Net;

namespace Vegas.AspNetCore.Common.Exceptions
{
    public class BusinessException : Exception
    {
        public const string MessageSeperator = "\n";
        public HttpStatusCode StatusCode { get; private set; }

        public BusinessException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }

        public BusinessException(HttpStatusCode statusCode, string message, Exception exception)
            : base(message, exception)
        {
            StatusCode = statusCode;
        }

        public BusinessException(HttpStatusCode statusCode, IEnumerable<string> messages)
            : base(string.Join(MessageSeperator, messages))
        {
            StatusCode = statusCode;
        }

        public BusinessException(HttpStatusCode statusCode, IEnumerable<string> messages, Exception exception)
            : base(string.Join(MessageSeperator, messages), exception)
        {
            StatusCode = statusCode;
        }

        public string[] Messages => Message.Split(MessageSeperator);
    }
}
