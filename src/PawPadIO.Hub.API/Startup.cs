// System and Microsoft
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// Third Party
using GraphQL;
using GraphQL.Authorization;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Validation;
using IdentityServer4.AccessTokenValidation;

// Internal
using PawPadIO.Hub.GraphQL;
using PawPadIO.Hub.Domain;
using PawPadIO.Hub.Domain.DAL;
using Microsoft.EntityFrameworkCore;
using PawPadIO.Hub.Domain.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using PawPadIO.Hub.API.Auth;

namespace PawPadIO.Hub.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment; // Add IWebHostEnvironment to private field so we can configure some services based on host environment (Development mode, etc)

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add required services
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDbContext<PawPadIODbContext>(options =>
            {
                options.UseSqlite("Data Source=sqlite.db"); // TODO: Allow other DBMSes to be used
            });

            // Adds a Secure Data Format handler to persist the state parameter into the server-side IDistributedCache
            services.AddOidcStateDataFormatterCache();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                options.Authority = "https://furry.nz/openid/";

                options.ClientId = "";
                options.ClientSecret = "";
                options.ResponseType = "code";

                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role",
                };

                options.Events.OnUserInformationReceived = NzFursOpenIdConnectEvents.OnUserInformationReceived;
            });

            // Add IdentityServer configuration
            // TODO: Build this from data store
            services.AddIdentityServer()
                .AddInMemoryApiResources(TempConfig.Apis)
                .AddInMemoryClients(TempConfig.Clients)
                //.AddAspNetIdentity<User>()
                //.AddTestUsers(TempConfig.TestUsers)
                .AddDeveloperSigningCredential(persistKey: false); //TODO: Replace this with an ECC key

            // Add GraphQL authorisation services
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            // Add GraphQL schema
            services.AddTransient<Query>();
            services.AddTransient<Mutation>();
            services.AddTransient<Subscription>();
            services.AddTransient<ISchema, GraphQL.Schema>();

            // TODO: Wrap this in an extension method so it doesn't look so naff
            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();
                authSettings.AddPolicy("ResidentPolicy", _ => _.RequireClaim("UserType", "Resident"));
                authSettings.AddPolicy("VerifiedGuestPolicy", _ => _.RequireClaim("UserType", "VerifiedGuest"));
                authSettings.AddPolicy("GuestPolicy", _ => _.RequireClaim("UserType", "Guest"));
                return authSettings;
            });

            // Add GraphQL Service
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true; // TODO: Allow this to be configurable via .toml, warn for security
                options.ExposeExceptions = _environment.IsDevelopment();
            })
            .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
            .AddUserContextBuilder(context => new UserContext { User = context.User })
            .AddWebSockets();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure extras allowed in development mode
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Use Authentication and Authorisation handlers
            app.UseAuthentication();

            // Use IdentityServer4 middleware
            app.UseIdentityServer();

            // Use GraphQL Transports middleware
            app.UseWebSockets();
            app.UseGraphQLWebSockets<ISchema>("/graphql");
            app.UseGraphQL<ISchema>("/graphql");

            // Show a default page if GraphQL endpoint isn't hit
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    if (env.IsDevelopment())
                    {
                        await context.Response.WriteAsync("PawPadIO API Server"); // TODO: Show list of endpoints
                    }
                    else
                    {
                        await context.Response.WriteAsync(""); // TODO: Redirect to web client application
                    }
                });
            });
        }
    }
}
