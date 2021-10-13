using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Vegas.AspNetCore.Common.Exceptions;
using Vegas.AspNetCore.Common.Models;
using Vegas.AspNetCore.Common.Settings;
using Vegas.NetCore.Common.Extensions;

namespace Vegas.AspNetCore.Common.Middlewares
{
    public class AutoRoutingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAutoRoutingSettings _autoRoutingSettings;

        public AutoRoutingMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IAutoRoutingSettings autoRoutingSettings)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
            _autoRoutingSettings = autoRoutingSettings;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var fromUri = httpContext.Request.Path.Value[1..];
            var fromMethod = httpContext.Request.Method;
            var autoRoute = _autoRoutingSettings.AutoRoutings
                .FirstOrDefault(x => x.FromRequestUri.ToLower() == fromUri.ToLower() &&
                                     x.HttpMethod.ToLower() == fromMethod.ToLower());
            if (autoRoute == null)
            {
                await _next(httpContext);
            }
            else
            {
                using var streamReader = new StreamReader(httpContext.Request.Body);
                var request = await streamReader.ReadToEndAsync();
                using var content = new StringContent(request, Encoding.UTF8, MediaTypeNames.Application.Json);
                var token = httpContext.Request.Headers["Authorization"];
                var queryString = httpContext.Request.QueryString.Value;
                var apiResponse = await SendAsync(autoRoute.FullAddress(queryString), autoRoute.HttpMethod, token, content);

                await httpContext.Response.WriteAsync(apiResponse.ToJson());
                //await httpContext.Response.WriteAsJsonAsync(apiResponse);
            }
        }

        public async Task<ApiResponse> SendAsync(string fullAddress, string method, string token, HttpContent content,
            CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(new HttpMethod(method), fullAddress)
            {
                Content = content,
            };
            httpRequest.Headers.Clear();
            if (AuthenticationHeaderValue.TryParse(token, out AuthenticationHeaderValue authenticationHeaderValue))
            {
                httpRequest.Headers.Authorization = authenticationHeaderValue;
            }

            var httpResponse = await _httpClientFactory.CreateClient("AutoRoutingClient").SendAsync(httpRequest, cancellationToken);
            return await HandleResponseAsync(httpResponse);
        }

        private static async Task<ApiResponse> HandleResponseAsync(HttpResponseMessage httpResponse)
        {
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonResponse))
            {
                throw new BusinessException(httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
            var apiResponse = jsonResponse.ToObject<ApiResponse>();
            if (!apiResponse.IsSuccess)
            {
                throw new BusinessException(httpResponse.StatusCode, apiResponse.Messages);
            }
            return apiResponse;
        }
    }
}
