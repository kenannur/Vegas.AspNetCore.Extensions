using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using Vegas.AspNetCore.Common.Exceptions;
using Vegas.AspNetCore.Common.Models;
using Vegas.AspNetCore.Localization.Localizer;
using Vegas.NetCore.Common.Extensions;

namespace Vegas.AspNetCore.Common.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        private readonly IJsonStringLocalizer _jsonStringLocalizer;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next,
                                                ILogger<GlobalExceptionHandlerMiddleware> logger,
                                                IJsonStringLocalizer jsonStringLocalizer)
        {
            _next = next;
            _logger = logger;
            _jsonStringLocalizer = jsonStringLocalizer;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);

                if (httpContext.Response.StatusCode != 200 && httpContext.Items["ErrorMessages"] is List<string> messages)
                {
                    throw new BusinessException((HttpStatusCode)httpContext.Response.StatusCode, messages);
                }
            }
            catch (BusinessException businessException)
            {
                _logger?.LogDebug(businessException, businessException.Message);
                await OverrideResponseAsync(httpContext, businessException.StatusCode, true, businessException.Messages);
            }
            catch (Exception exception)
            {
                _logger?.LogError(exception, exception.Message);
                await OverrideResponseAsync(httpContext, HttpStatusCode.InternalServerError, false, exception.Message);
            }
        }

        private async Task OverrideResponseAsync(HttpContext httpContext, HttpStatusCode code, bool isLocalizable,
            params string[] messages)
        {
            var messageList = new List<string>();
            if (isLocalizable && _jsonStringLocalizer != null)
            {
                var requestCultureName = httpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name;
                foreach (var message in messages)
                {
                    var localizedMessage = _jsonStringLocalizer.GetString(message, requestCultureName);
                    messageList.Add(localizedMessage);
                }
            }
            else
            {
                messageList.AddRange(messages);
            }

            var jApiResponse = new ApiResponse
            {
                IsSuccess = false,
                Messages = messageList
            }.ToJson();

            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            httpContext.Response.StatusCode = (int)code;
            await httpContext.Response.WriteAsync(jApiResponse);
        }
    }
}
