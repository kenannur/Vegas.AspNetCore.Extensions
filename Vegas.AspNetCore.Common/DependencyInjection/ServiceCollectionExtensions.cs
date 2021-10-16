using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vegas.AspNetCore.Common.Settings;
using Vegas.AspNetCore.Configuration.Extensions;

namespace Vegas.AspNetCore.Common.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds AutoRoutingSettings where defined in appsettings.json to IServiceCollection as Singleton
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoRouting(this IServiceCollection services, IConfiguration configuration)
        {
            //services.ConfigureSettings<IAutoRoutingSettings, AutoRoutingSettings>(configuration);
            services.AddNamedHttpClient();
            return services;
        }

        /// <summary>
        /// Adds (AutoRoutingSettings : IAutoRoutingSettings) settings to IServiceCollection as Singleton
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoRouting(this IServiceCollection services, IAutoRoutingSettings settings)
        {
            services.AddSingleton(settings);
            services.AddNamedHttpClient();
            return services;
        }

        private static void AddNamedHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("AutoRoutingClient");
        }
    }
}
