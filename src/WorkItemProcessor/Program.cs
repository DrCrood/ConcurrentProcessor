using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using WorkItemProcessor.Interfaces;
using WorkItemProcessor.Services;

namespace WorkItemProcessor
{
    public static class Program
    {
        public async static Task Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(config =>
                {
                  //The default builder read DOTNET_ prefixed variable only. 
                  config.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                })
                .UseWindowsService(options =>
                {
                    options.ServiceName = "WorkItem-Processor";
                })
                .ConfigureServices(services =>
                {
                    if(OperatingSystem.IsWindows())
                    {
                        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);
                    }
                    services.AddHostedService<WorkItemProcessManager>();
                    services.AddSingleton<IConfigurationService, ConfigurationService>();
                    services.AddSingleton<IUtilityService, UtilityService>();
                    services.AddSingleton<IWorkItemMonitoringService, WorkItemMonitoringService>();
                    services.AddSingleton<IWorkItemCollection, WorkItemCollection>();
                    services.AddSingleton<IQueuedWorkItemProcessor, QueuedWorkItemProcessor>();
                })
                .ConfigureLogging((context, logging) =>
                {
                   logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                })
                .Build();

            await host.RunAsync();
        }
    }
}