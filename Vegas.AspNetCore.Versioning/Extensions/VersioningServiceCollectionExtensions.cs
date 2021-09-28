using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Vegas.AspNetCore.Versioning.Extensions
{
    public static class VersioningServiceCollectionExtensions
    {
        public static void AddApiVersioning<TApiVersionReader>(this IServiceCollection services, string version = default)
            where TApiVersionReader : IApiVersionReader
        {
            var isNotEmpty = !string.IsNullOrWhiteSpace(version);
            var isParsable = ApiVersion.TryParse(version, out ApiVersion apiVersion);
            var defaultVersion = isNotEmpty && isParsable ? apiVersion : ApiVersion.Default;

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = defaultVersion;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = Activator.CreateInstance<TApiVersionReader>();
            });
        }
    }
}
