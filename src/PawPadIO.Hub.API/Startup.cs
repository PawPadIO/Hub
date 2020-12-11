using System.Net.Http;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PawPadIO.Hub.API.ServiceDescriptors;

namespace PawPadIO.Hub.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(options =>
            {
                
            })
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


            services.AddGraphQLServer(exposeDebugInfo: _environment.IsDevelopment());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthorization();

            // Use GraphQL Transports middleware
            app.UseWebSockets();
            app.UseGraphQLWebSockets<ISchema>("/graphql");
            app.UseGraphQL<ISchema>("/graphql");
        }
    }
}
