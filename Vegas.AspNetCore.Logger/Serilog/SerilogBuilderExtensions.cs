using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Vegas.AspNetCore.Logger.Serilog
{
    public static class SerilogBuilderExtensions
    {
        public static IHostBuilder ConfigureAndUseSerilog(this IHostBuilder hostBuilder)
        {
            try
            {
                ConfigureSerilog();

                hostBuilder = hostBuilder.ConfigureLogging(logging => logging.ClearProviders())
                                         .UseSerilog();

                Log.Information("Serilog configured");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "While Serilog configuring an error occured");
            }
            return hostBuilder;
        }

        private static void ConfigureSerilog()
        {
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var directoryName = Path.GetDirectoryName(assemblyLocation);
            var logFilePath = $"{directoryName}/Log/log.txt";

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                //.Enrich.WithProperty("AppName", "AppName")
                //.Enrich.WithProperty("Environment", "DEV")
                .WriteTo.File(logFilePath)
                .CreateLogger();
        }
    }
}
