// System and Microsoft
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

// Third Party
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Validation;
using GraphQL.Authorization;

// Internal
using PawPadIO.Hub.GraphQL;
using PawPadIO.Hub.Domain;

namespace PawPadIO.Hub.API
{
    public class Startup
    {
        // Add IWebHostEnvironment to private field so we can configure some services based on host environment (Development mode, etc)
        private readonly IWebHostEnvironment _environment;

        public Startup(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add required services
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add IdentityServer configuration
            // TODO: Build this from data store
            services.AddIdentityServer()
                .AddInMemoryApiResources(TempConfig.Apis)
                .AddInMemoryClients(TempConfig.Clients)
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
