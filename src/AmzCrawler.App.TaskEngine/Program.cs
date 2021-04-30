using AmzCrawler.App.Services.Configurations;
using AmzCrawler.App.Services.Helpers;
using AmzCrawler.App.Services.Services;
using AmzCrawler.App.Services.Services.Interfaces;
using AmzCrawler.App.TaskEngine.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NDesk.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmzCrawler.App.TaskEngine
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var taskInput = new TaskInputModel();
            int? taskId = -1;
            var options = new OptionSet
                    {
                        {"c=", v => taskInput.Command = v},
                        {"ssi=", v => taskInput.SpreadsheetId = v},
                        {"si=", v => taskInput.SheetId = v.ToNullableInt()},
                        {"sn=", v => taskInput.SheetName = v},
                        {"ti=", v => taskInput.ApifyTaskId = v},
                        {"tk=", v => taskInput.ApifyToken = v},
                        {"s=", v => taskInput.StartIndex = v.ToNullableInt()},
                        {"e=", v => taskInput.EndIndex = v.ToNullableInt()},
                        {"fs=", v => taskInput.FailedToSyncOnly = v.ToNullableBoolean()},
                        {"id=", v => taskId = v.ToNullableInt()},
                    };
            options.Parse(args);


            using var scope = ConfigureServiceScope();
            var services = scope.ServiceProvider;
            var taskExecutor = services.GetRequiredService<ITaskExecutor>();
            await taskExecutor.Excute(taskId.GetValueOrDefault());
        }

        private static IServiceScope ConfigureServiceScope()
        {
            var host = new HostBuilder()
                         .ConfigureAppConfiguration((context, builder) =>
                         {
                             builder.AddJsonFile("appsettings.json", optional: true);
                         })
                         .ConfigureServices((context, services) =>
                         {
                             services.Configure<GoogleConfiguration>(context.Configuration.GetSection(nameof(GoogleConfiguration)))
                                     .Configure<ApifyConfiguration>(context.Configuration.GetSection(nameof(ApifyConfiguration)))
                                     .AddSingleton(context.Configuration.GetSection("TaskList").Get<List<TaskInputModel>>())
                                     .AddScoped<IGoogleSheetService, GoogleSheetService>()
                                     .AddScoped<IApifyService, ApifyService>()
                                     .AddScoped<ITaskExecutor, TaskExecutor>();
                         });

            return host.Build().Services.CreateScope();
        }
    }
}
