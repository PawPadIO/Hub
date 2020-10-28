using GraphQL.Authorization;
using GraphQL.Server;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PawPadIO.Hub.Web.ServiceDescriptors
{
    public static class GraphQLServerDescriptor
    {
        public static void AddGraphQLServer(this IServiceCollection services, bool exposeExceptionStackTrace)
        {
            // Add GraphQL schema
            services.AddTransient<PawPadIO.Hub.GraphQL.Query>();
            services.AddTransient<PawPadIO.Hub.GraphQL.Mutation>();
            services.AddTransient<PawPadIO.Hub.GraphQL.Subscription>();
            services.AddTransient<ISchema, PawPadIO.Hub.GraphQL.Schema>();

            // Add GraphQL authorisation services
            //services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, global::GraphQL.Server.Authorization.AspNetCore.AuthorizationValidationRule>();

            // Add GraphQL Service
            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true; // TODO: Allow this to be configurable via .toml, warn for security
            })
            .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = exposeExceptionStackTrace)
            //.AddGraphQLAuthorization(options =>
            //{
            //    options.AddPolicy("graphql", policy => // Just a test one
            //    {
            //        policy.AuthenticationSchemes.Clear();
            //        policy.AddAuthenticationSchemes("token");
            //        policy.RequireAuthenticatedUser();
            //    });
            //})
            .AddUserContextBuilder(context =>
            {
                // TODO: This is our issue here, this is not populated with the user's claims and authenticated despite having the valid header
                // Do we need to somehow invoke the "token" Authentication Scheme manually?

                return new PawPadIO.Hub.GraphQL.UserContext
                {
                    User = context.User
                };
            })
            .AddWebSockets();
        }
    }
}
