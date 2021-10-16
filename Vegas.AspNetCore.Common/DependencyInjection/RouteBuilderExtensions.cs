using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace Vegas.AspNetCore.Common.DependencyInjection
{
    public static class RouteBuilderExtensions
    {
        public static void MapDefaults(this IRouteBuilder endpoints, IHostEnvironment env)
        {
            endpoints.MapGet("/", async context =>
            {
                await context.Response.WriteAsync($"{env.ApplicationName} is running: {DateTime.Now}");
            });
            endpoints.MapGet("/ex", context =>
            {
                throw new Exception("message");
            });
        }

    }
}
