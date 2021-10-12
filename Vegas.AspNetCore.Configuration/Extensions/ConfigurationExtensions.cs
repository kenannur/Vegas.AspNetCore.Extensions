using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Vegas.AspNetCore.Configuration.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection ConfigureSettings<TInterface, TSettings>(this IServiceCollection services,
            IConfiguration configuration)
            where TInterface : class
            where TSettings : class, TInterface
        {
            configuration.ThrowIfNotExists<TSettings>();
            services.Configure<TSettings>(configuration.GetSection(typeof(TSettings).Name));
            return services.AddSingleton<TInterface>(sp => sp.GetRequiredService<IOptions<TSettings>>().Value);
        }

        public static bool Exists<TSettings>(this IConfiguration configuration)
            => configuration.GetSection(typeof(TSettings).Name).Exists();

        public static bool Exists(this IConfiguration configuration, string name)
            => configuration.GetSection(name).Exists();

        public static void ThrowIfNotExists<TSettings>(this IConfiguration configuration)
        {
            var settingsTypeName = typeof(TSettings).Name;
            var configurationSection = configuration.GetSection(settingsTypeName);
            if (!configurationSection.Exists())
            {
                var message = $"{settingsTypeName} section not found in appsettings.json";
                throw new ConfigurationErrorsException(message);
            }
        }
    }
}
