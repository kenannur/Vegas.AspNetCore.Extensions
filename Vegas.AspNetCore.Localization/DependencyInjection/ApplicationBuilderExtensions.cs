using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

namespace Vegas.AspNetCore.Localization.DependencyInjection
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseJsonLocalization(this IApplicationBuilder app,
            List<string> cultures, string defaultCulture = default)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }
            if (cultures == null || !cultures.Any())
            {
                throw new ArgumentNullException(nameof(cultures));
            }
            var cultureInfos = cultures.Select(x => new CultureInfo(x)).ToList();
            app.UseRequestLocalization(options =>
            {
                options.SupportedCultures = cultureInfos;
                options.SupportedUICultures = cultureInfos;
                options.DefaultRequestCulture = new RequestCulture(defaultCulture ?? cultures.FirstOrDefault());
            });
            return app;
        }
    }
}
