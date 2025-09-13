using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WorkflowTrackingSystem.Infrastructure.Logging
{
    public static class LoggerConfig
    {
        public static void ConfigureLogger(HostBuilderContext context, LoggerConfiguration configuration)
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext();
        }
    }
}
