using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;

namespace Vegas.AspNetCore.Common.DependencyInjection
{
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds autoRouting.*.json format files to IConfigurationBuilder
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static IConfigurationBuilder ConfigureAutoRouting(this IConfigurationBuilder config,
            IHostEnvironment hosting, string folder = default)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (!string.IsNullOrWhiteSpace(folder))
            {
                basePath = Path.Combine(basePath, folder);
            }

            var filePaths = Directory.EnumerateFiles(basePath, "autoRouting.*.json", SearchOption.AllDirectories);
            if (hosting.IsDevelopment())
            {
                filePaths = filePaths.Where(x => x.Contains(hosting.EnvironmentName));
            }
            else
            {
                filePaths = filePaths.Where(x => !x.Contains(hosting.EnvironmentName));
            }
            var jArray = new JArray();
            foreach (var filePath in filePaths)
            {
                var content = File.ReadAllText(filePath);
                jArray.Merge(JArray.Parse(content));
            }

            var jAutoRoutingsObject = new JObject
            {
                { "AutoRoutings", jArray }
            };
            var jResultObject = new JObject
            {
                { "AutoRoutingSettings", jAutoRoutingsObject }
            };
            var jsonBytes = Encoding.UTF8.GetBytes(jResultObject.ToString());
            var jsonStream = new MemoryStream(jsonBytes);
            return config.AddJsonStream(jsonStream);
        }
    }
}
