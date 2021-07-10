using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Threading.Tasks;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using PawPadIO.Hub.API.Auth;
using PawPadIO.Hub.API.ServiceDescriptors;
using PawPadIO.Hub.Domain.Data;
using PawPadIO.Hub.Domain.Models;
using PawPadIO.Hub.Domain.Services;
using PawPadIO.Hub.GraphQL.Types;

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
            services.AddTransient<IUserService<HubUser>, PawPadIOUserService>();
            services.AddTransient<HubUserType>();

            // Use the JWT standard claim names, not the silly xmlsoap URIs
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Show full IdentityModel logs if in Development environment
            IdentityModelEventSource.ShowPII = _environment.IsDevelopment();

            services.AddDbContext<HubDbContext>(options =>
                options.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
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
                    options.IncludeErrorDetails = true;
                }
                else
                {
                    options.IncludeErrorDetails = false;
                }

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "given_name",
                    RoleClaimType = "role",
                };

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = PawPadIOJwtBearerEvents.OnAuthenticationFailed,
                    OnChallenge = PawPadIOJwtBearerEvents.OnChallenge,
                    OnForbidden = PawPadIOJwtBearerEvents.OnForbidden,
                    OnTokenValidated = PawPadIOJwtBearerEvents.OnTokenValidated,
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("graphql", policy => // HACK: Just a test one, remove this later
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                });
                options.AddPolicy("ResidentPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim("UserType", "Resident");
                });
                options.AddPolicy("VerifiedGuestPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireClaim("UserType", "VerifiedGuest");
                });
                options.AddPolicy("GuestPolicy", policy =>
                {
                    policy.AuthenticationSchemes.Clear();
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
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

            app.UseAuthentication();
            app.UseAuthorization();

            // Use GraphQL Transports middleware
            app.UseWebSockets();
            app.UseGraphQLWebSockets<ISchema>("/graphql");
            app.UseGraphQL<ISchema>("/graphql");
        }
    }
}
