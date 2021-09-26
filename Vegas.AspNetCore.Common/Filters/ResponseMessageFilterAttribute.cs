using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Vegas.AspNetCore.Common.Models;
using Vegas.AspNetCore.Localization.Localizer;

namespace Vegas.AspNetCore.Common.Filters
{
    public class ResponseMessageFilterAttribute : ResultFilterAttribute
    {
        private readonly string _message;
        private readonly IJsonStringLocalizer _jsonStringLocalizer;

        public ResponseMessageFilterAttribute(IJsonStringLocalizer jsonStringLocalizer, string message)
        {
            _jsonStringLocalizer = jsonStringLocalizer;
            _message = message;
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Result is OkObjectResult result && result.Value is ApiResponse apiResponse)
            {
                var localizedMessage = _jsonStringLocalizer.GetString(_message);
                apiResponse.Messages.Add(localizedMessage);
            }
            return base.OnResultExecutionAsync(context, next);
        }
    }

    public class ResponseMessageAttribute : TypeFilterAttribute
    {
        public ResponseMessageAttribute(string messageKey) : base(typeof(ResponseMessageFilterAttribute))
        {
            Arguments = new object[] { messageKey };
        }
    }
}
