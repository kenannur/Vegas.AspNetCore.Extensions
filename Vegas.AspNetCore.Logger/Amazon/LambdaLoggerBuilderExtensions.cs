using AWS.Logger;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Vegas.AspNetCore.Logger.Amazon
{
    public static class LambdaLoggerBuilderExtensions
    {
        public static IHostBuilder ConfigureLambdaLogger(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging(logging =>
            {
                var loggerOptions = new LambdaLoggerOptions
                {
                    IncludeException = true,
                    //Filter = (_, logLevel) => logLevel >= LogLevel.Warning,
                };
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Warning);
                logging.AddLambdaLogger(loggerOptions);
            });
        }

        public static IHostBuilder ConfigureLocalLambdaLogger(this IHostBuilder hostBuilder, string logGroupName)
        {
            return hostBuilder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Warning);
                logging.AddAWSProvider(new AWSLoggerConfig(logGroupName));
            });
        }
    }
}
