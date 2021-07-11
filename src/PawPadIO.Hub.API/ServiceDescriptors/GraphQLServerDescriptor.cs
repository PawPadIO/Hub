using GraphQL.Server;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace PawPadIO.Hub.API.ServiceDescriptors
{
    public static class GraphQLServerDescriptor
    {
        public static void AddGraphQLServer(this IServiceCollection services, bool exposeDebugInfo = false)
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
                options.EnableMetrics = exposeDebugInfo; // TODO: Allow this to be configurable via .toml, warn for security
            })
            .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { })
            .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = exposeDebugInfo)
            .AddGraphQLAuthorization()
            //    options =>
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

                var graphQLUserContext = new GraphQL.UserContext(context.User);

                return graphQLUserContext;
            })
            .AddWebSockets();
        }
    }
}
