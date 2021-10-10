using System;
using AWS.Logger;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Vegas.AspNetCore.Logger.Amazon
{
    public static class LambdaLoggerBuilderExtensions
    {
        [Obsolete("This method deprecated. Use 'logging.UseAmazonCloudWatch(...)' extension")]
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

        [Obsolete("This method deprecated. Use 'logging.UseAmazonCloudWatch(...)' extension")]
        public static IHostBuilder ConfigureLocalLambdaLogger(this IHostBuilder hostBuilder, string logGroupName)
        {
            return hostBuilder.ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                logging.SetMinimumLevel(LogLevel.Warning);
                logging.AddAWSProvider(new AWSLoggerConfig(logGroupName));
            });
        }

        public static ILoggingBuilder UseAmazonCloudWatch(this ILoggingBuilder logging,
            IHostEnvironment environment, LogLevel minimumLevel = LogLevel.Warning, string logGroupName = "")
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(minimumLevel);
            if (environment.IsProduction())
            {
                var loggerOptions = new LambdaLoggerOptions
                {
                    IncludeException = true,
                    //Filter = (_, logLevel) => logLevel >= LogLevel.Warning,
                };
                logging.AddLambdaLogger(loggerOptions);
            }
            else
            {
                var awsLoggerConfig = new AWSLoggerConfig();
                if (!string.IsNullOrWhiteSpace(logGroupName))
                {
                    awsLoggerConfig.LogGroup = logGroupName;
                }
                logging.AddAWSProvider(awsLoggerConfig);
            }
            return logging;
        }
    }
}
