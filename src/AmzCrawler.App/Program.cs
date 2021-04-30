using AmzCrawler.App.Services.Configurations;
using AmzCrawler.App.Services.Services;
using AmzCrawler.App.Services.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Windows.Forms;

namespace AmzCrawler.App
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using var scope = ConfigureServiceScope();
            var services = scope.ServiceProvider;
            var mainForm = services.GetRequiredService<MainForm>();
            Application.Run(mainForm);
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
                                     .AddSingleton<MainForm>()
                                     .AddScoped<IGoogleSheetService, GoogleSheetService>()
                                     .AddScoped<IApifyService, ApifyService>()
                                     .AddLogging(configure => configure.AddConsole());
                         });

            return host.Build().Services.CreateScope();
        }
    }
}
