using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Vegas.AspNetCore.Localization.Localizer;
using Vegas.AspNetCore.Localization.Options;

namespace Vegas.AspNetCore.Localization.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services,
            Action<JsonStringLocalizerOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }
            services.Configure(setupAction);
            services.TryAddSingleton<IJsonStringLocalizer, JsonStringLocalizer>();
            return services;
        }
    }
}
