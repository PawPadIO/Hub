using System.Linq;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PawPadIO.Hub.Domain.Data;
using PawPadIO.Hub.Domain.Models;
using PawPadIO.Hub.Domain.Services;
using PawPadIO.Hub.GraphQL.Types;

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
                    var dbContext = context.RequestServices.GetService<HubDbContext>();
                    var userService = context.RequestServices.GetService<IUserService<HubUser>>();

                    var userContext = (UserContext)context.UserContext;
                    var user = await userService.GetUserFromIssuerAsync(userContext.Issuer, userContext.Subject, context.CancellationToken);

                    var loggedInString = (user != null) ? "Logged in as " + user.Name : "Not logged in";

                    var userCount = dbContext.HubUsers.Count();

                    return loggedInString + $"({userCount} total)";
                }
            ).AuthorizeWith("graphql");

            FieldAsync<ListGraphType<HubUserType>>(
                name: "users",
                description: "All current users.",
                resolve: async context =>
                {
                    var dbContext = context.RequestServices.GetService<HubDbContext>();

                    var users = await dbContext.HubUsers.ToListAsync();

                    return users;
                }
           ).AuthorizeWith("graphql");
        }
    }
}
