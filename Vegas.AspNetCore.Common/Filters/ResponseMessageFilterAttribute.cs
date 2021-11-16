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
            context.Result.Is<OkObjectResult>()?.Messages.Add(_jsonStringLocalizer.GetString(_message));
            context.Result.Is<AcceptedResult>()?.Messages.Add(_jsonStringLocalizer.GetString(_message));
            context.Result.Is<CreatedResult>()?.Messages.Add(_jsonStringLocalizer.GetString(_message));

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

    public static class ActionResultExtensions
    {
        public static ApiResponse Is<TActionResult>(this IActionResult actionResult) where TActionResult : ObjectResult
        {
            if (actionResult is TActionResult result && result.Value is ApiResponse apiResponse)
            {
                return apiResponse;
            }
            return null;
        }
    }
}
