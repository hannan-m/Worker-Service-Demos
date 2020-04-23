using System.Threading.Tasks;
using BackgroundTasksSample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundTasksSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();

            await host.StartAsync();

            #region snippet4

//            var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
//            monitorLoop.StartMonitorLoop();

            #endregion

            // Wait for the host to shutdown
            await host.WaitForShutdownAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
//                .UseConsoleLifetime(options => options.SuppressStatusMessages = true)
//                .ConfigureLogging(builder => { builder.AddEventLog(); })
                .ConfigureServices((hostContext, services) =>
                {
                    #region snippet1
//
                    services.AddHostedService<TimedHostedService>();
//
                    #endregion
//
                    #region snippet2
//
//                    services.AddHostedService<ConsumeScopedServiceHostedService>();
//                    services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
//
                    #endregion
  
                    #region snippet3

//                    services.AddSingleton<MonitorLoop>();
//                    services.AddHostedService<QueuedHostedService>();
//                    services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();

                    #endregion
                });
    }
}