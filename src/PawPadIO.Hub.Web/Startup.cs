using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using IdentityServer4.Services;
using PawPadIO.Hub.Web.Models;
using PawPadIO.Hub.Web.Data;
using PawPadIO.Hub.Web.Resources;
using PawPadIO.Hub.Web.Services;
using PawPadIO.Hub.Web.Filters;
using PawPadIO.Hub.Web.Services.Certificate;
using Serilog;
using Fido2NetLib;
using PawPadIO.Hub.Web.ServiceDescriptors;
using System.Net.Http;

namespace PawPadIO.Hub.Web
{
    public class Startup
    {
        private IConfiguration _configuration { get; }
        private IWebHostEnvironment _environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<EmailSettings>(_configuration.GetSection("EmailSettings"));
            services.AddTransient<IProfileService, IdentityWithAdditionalClaimsProfileService>();
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddCookiePolicyOptions();

            var x509Certificate2Certs = GetCertificates(_environment, _configuration)
                .GetAwaiter().GetResult();
            services.AddLocalizationConfigurations();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<StsIdentityErrorDescriber>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<Fifo2UserTwoFactorTokenProvider>("FIDO2");

            // Enable to prevent ASP.NET from renaming scopes to stupid names
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication()
                .AddJwtBearer("token", options =>
                {
                    options.Authority = "https://127.0.0.1:5001"; // TODO: Set this properly
                    options.Audience = "nz.furry.infursec.hub"; // TODO: Set this properly
                    options.RequireHttpsMetadata = false;
                    if (_environment.IsDevelopment())
                    {
                        // Allow certificates that are untrusted/invalid
                        options.BackchannelHttpHandler = new HttpClientHandler
                        {
                            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                        };
                    }
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("graphql", policy => // HACK: Just a test one, remove this later
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes("token");
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("ResidentPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes("token");
                    policy.RequireClaim("UserType", "Resident");
                });
                options.AddPolicy("VerifiedGuestPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes("token");
                    policy.RequireClaim("UserType", "VerifiedGuest");
                });
                options.AddPolicy("GuestPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes("token");
                    policy.RequireClaim("UserType", "Guest");
                });
            });

            services.AddWebSecurity();

            services.AddControllersWithViews(options =>
                {
                    options.Filters.Add(new SecurityHeadersAttribute());
                })
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("SharedResource", assemblyName.Name);
                    };
                })
                .AddNewtonsoftJson(); // TODO: We want to remove this if we can, we should use System.Text.Json

            var identityServer = services.AddIdentityServer()
                .AddSigningCredential(x509Certificate2Certs.ActiveCertificate)
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityWithAdditionalClaimsProfileService>()
                .AddJwtBearerClientAuthentication();

            if (x509Certificate2Certs.SecondaryCertificate != null)
            {
                identityServer.AddValidationKey(x509Certificate2Certs.SecondaryCertificate);
            }

            services.Configure<Fido2Configuration>(_configuration.GetSection("fido2"));
            services.AddScoped<Fido2Storage>();
            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseCookiePolicy();

            if (_environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            app.UseCsp(opts => opts
                .BlockAllMixedContent()
                .StyleSources(s => s.Self())
                .StyleSources(s => s.UnsafeInline())
                .FontSources(s => s.Self())
                .FrameAncestors(s => s.Self())
                .ImageSources(imageSrc => imageSrc.Self())
                .ImageSources(imageSrc => imageSrc.CustomSources("data:"))
                .ScriptSources(s => s.Self())
                .ScriptSources(s => s.UnsafeInline())
            );

            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            // https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/
            // https://nblumhardt.com/2019/10/serilog-mvc-logging/
            app.UseSerilogRequestLogging();

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    if (context.Context.Response.Headers["feature-policy"].Count == 0)
                    {
                        var featurePolicy = "accelerometer 'none'; camera 'none'; geolocation 'none'; gyroscope 'none'; magnetometer 'none'; microphone 'none'; payment 'none'; usb 'none'";

                        context.Context.Response.Headers["feature-policy"] = featurePolicy;
                    }

                    if (context.Context.Response.Headers["X-Content-Security-Policy"].Count == 0)
                    {
                        var csp = "script-src 'self';style-src 'self';img-src 'self' data:;font-src 'self';form-action 'self';frame-ancestors 'self';block-all-mixed-content";
                        // IE
                        context.Context.Response.Headers["X-Content-Security-Policy"] = csp;
                    }
                }
            });

            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static async Task<(X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate)> GetCertificates(IWebHostEnvironment environment, IConfiguration configuration)
        {
            var certificateConfiguration = new CertificateConfiguration
            {
                // Use an Azure key vault
                CertificateNameKeyVault = configuration["CertificateNameKeyVault"], //"StsCert",
                KeyVaultEndpoint = configuration["AzureKeyVaultEndpoint"], // "https://damienbod.vault.azure.net"

                // Use a local store with thumbprint
                //UseLocalCertStore = Convert.ToBoolean(configuration["UseLocalCertStore"]),
                //CertificateThumbprint = configuration["CertificateThumbprint"],

                // development certificate
                DevelopmentCertificatePfx = Path.Combine(environment.ContentRootPath, "sts_dev_cert.pfx"),
                DevelopmentCertificatePassword = "1234" //configuration["DevelopmentCertificatePassword"] //"1234",
            };

            (X509Certificate2 ActiveCertificate, X509Certificate2 SecondaryCertificate) certs = await CertificateService.GetCertificates(
                certificateConfiguration).ConfigureAwait(false);

            return certs;
        }
    }
}
