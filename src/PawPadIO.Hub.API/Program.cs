using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PawPadIO.Hub.Domain.DAL;

namespace PawPadIO.Hub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Get the HTTP Server set up
            var host = CreateHostBuilder(args).Build();

            // Check to see if the DB has been created yet (do this if we haven't already)
            // TODO: Migrations
            CreateDbIfNotExists(host);

            // Run the HTTP Server
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((hostContext, config) =>
                    {
                        // TODO: Allow TOML config (makes this way easier for new users)
                        // config.AddTomlFile(config.GetFileProvider(), "appsettings.toml", optional: true, true);
                        // config.AddTomlFile(config.GetFileProvider(), $"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.toml", optional: true, true);
                    });
                    // TODO: Config validation: https://github.com/InFurSecDen/ConfigurationValidator
                    webBuilder.UseKestrel(options =>
                    {
                        // TODO: Lots of work to do here to:
                        // * Make HTTPS only, no HTTP
                        // * Secure protocols/ciphers
                        // * Configure cert to use (LE by default?)
                        // * Configure port
                        options.ConfigureHttpsDefaults(httpsOptions => 
                        {
                            httpsOptions.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
                        });
                    });
                });

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<PawPadIODbContext>();
                PawPadIODbContextInitialiser.Initialise(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occured while creating the database.");
            }
        }
    }
}
