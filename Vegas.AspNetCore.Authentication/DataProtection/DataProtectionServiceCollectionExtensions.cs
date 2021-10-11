using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Vegas.AspNetCore.Authentication.DataProtection
{
    public static class DataProtectionServiceCollectionExtensions
    {
        public static void ConfigureKeyManagementOptions(this IServiceCollection services)
        {
            services.Configure<KeyManagementOptions>(options =>
            {
                options.XmlEncryptor = new VegasXmlEncryptor();
            });
        }
    }
}
