using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Vegas.AspNetCore.Common.DependencyInjection
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds autoRouting.*.json format files to IConfigurationBuilder
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static IHostBuilder ConfigureAutoRoutingConfiguration(this IHostBuilder hostBuilder, string folder = default)
        {
            return hostBuilder.ConfigureAppConfiguration((hosting, config) =>
            {
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (!string.IsNullOrWhiteSpace(folder))
                {
                    basePath = Path.Combine(basePath, folder);
                }
                config.SetBasePath(basePath);

                var filePaths = Directory.GetFiles(basePath, "autoRouting.*.json", SearchOption.AllDirectories);
                foreach (var filePath in filePaths)
                {
                    var fileName = filePath.Split("/").Last();
                    config.AddJsonFile(fileName, true, true);
                }
            });
        }
    }
}
