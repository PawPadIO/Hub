using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PawPadIO.Hub.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
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
    }
}
