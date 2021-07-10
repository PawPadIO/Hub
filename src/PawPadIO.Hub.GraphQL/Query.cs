using System.Linq;
using GraphQL.Authorization;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PawPadIO.Hub.Domain.Data;
using PawPadIO.Hub.Domain.Models;
using PawPadIO.Hub.Domain.Services;

namespace PawPadIO.Hub.GraphQL
{
    public class Query : ObjectGraphType<object>
    {
        public Query(IHttpContextAccessor httpContextAccessor)
        {
            Field<StringGraphType>(
                name: "test",
                description: "A test query.",
                resolve: context => "Test Result"
            );

            FieldAsync<StringGraphType>(
                name: "user",
                description: "The current user.",
                resolve: async context =>
                {
                    // TODO: Get GraphQL.MicrosoftDI up and running
                    var userService = context.RequestServices.GetService<IUserService<HubUser>>();

                    var userContext = (UserContext)context.UserContext;
                    var user = await userService.GetUserFromIssuerAsync(userContext.Issuer, userContext.Subject, context.CancellationToken);

                    var loggedInString = (user != null) ? "Logged in as " + user.Name : "Not logged in";

                    var dbContext = context.RequestServices.GetService<HubDbContext>();

                    var userCount = dbContext.HubUsers.Count();

                    return loggedInString + $"({userCount} total)";
                }
           ).AuthorizeWith("graphql");
        }
    }
}
