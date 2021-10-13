using Microsoft.AspNetCore.Builder;
using Vegas.AspNetCore.Common.Middlewares;

namespace Vegas.AspNetCore.Common.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Should be first middleware for whole exceptions
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        }

        /// <summary>
        /// Must be used between app.UseEndpoints(...) middleware and app.UseAuthorization() middleware
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseAutoRouting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AutoRoutingMiddleware>();
        }
    }
}
