using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Vegas.AspNetCore.Localization.Localizer;
using Vegas.AspNetCore.Localization.Options;

namespace Vegas.AspNetCore.Localization.DependencyInjection
{
    public static class LocalizationServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonLocalization(this IServiceCollection services,
            List<string> cultures,
            Action<JsonStringLocalizerOptions> jsonStringLocalizerSetupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (cultures == null || !cultures.Any())
            {
                throw new ArgumentNullException(nameof(cultures));
            }
            if (jsonStringLocalizerSetupAction == null)
            {
                throw new ArgumentNullException(nameof(jsonStringLocalizerSetupAction));
            }

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultureInfoList = new List<CultureInfo>();
                foreach (var culture in cultures)
                {
                    cultureInfoList.Add(new CultureInfo(culture));
                }
                options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault());
                options.SupportedCultures = cultureInfoList;
                options.SupportedUICultures = cultureInfoList;
            });
            services.Configure(jsonStringLocalizerSetupAction);
            services.TryAddSingleton<IJsonStringLocalizer>(sp =>
            {
                return new JsonStringLocalizer(sp.GetRequiredService<IOptions<JsonStringLocalizerOptions>>(),
                                               sp.GetRequiredService<IOptions<RequestLocalizationOptions>>());
            });
            return services;
        }

        public static IServiceCollection AddJsonLocalization(this IServiceCollection services,
            IConfiguration configuration, string resourceName)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentNullException(nameof(resourceName));
            }
            var localizerDictionary = configuration.GetSection(resourceName).Get<Dictionary<string, Dictionary<string, string>>>();
            var cultures = localizerDictionary.Keys;

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var cultureInfoList = new List<CultureInfo>();
                foreach (var culture in cultures)
                {
                    cultureInfoList.Add(new CultureInfo(culture));
                }
                options.DefaultRequestCulture = new RequestCulture(cultures.FirstOrDefault());
                options.SupportedCultures = cultureInfoList;
                options.SupportedUICultures = cultureInfoList;
            });
            services.TryAddSingleton<IJsonStringLocalizer>(sp =>
            {
                return new JsonStringLocalizer(localizerDictionary);
            });
            return services;
        }
    }
}
