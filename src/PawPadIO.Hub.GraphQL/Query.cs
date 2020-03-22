using Microsoft.AspNetCore.Http;
using GraphQL.Types;

namespace PawPadIO.Hub.GraphQL
{
    public class Query : ObjectGraphType<object>
    {
        public Query(
            IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor.HttpContext.User;

            Field<StringGraphType>(
                name: "test",
                description: "A test query.",
                resolve: context => "Test Result"
            );
        }
    }
}
