// System and Microsoft
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Third Party
using GraphQL;
using GraphQL.Server;
using GraphQL.Types;

// Internal


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
            // Add GraphQL schema
            services.AddTransient<GraphQL.Query>();
            services.AddTransient<GraphQL.Mutation>();
            services.AddTransient<GraphQL.Subscription>();
            services.AddTransient<ISchema, GraphQL.Schema>();

            // Add GraphQL Service
            services.AddSingleton<IDependencyResolver>(c => new FuncDependencyResolver(type => c.GetRequiredService(type)));
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true; // TODO: Allow this to be configurable via .toml, warn for security
                options.ExposeExceptions = _environment.IsDevelopment();
            })
            .AddWebSockets();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Configure extras allowed in development mode
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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
