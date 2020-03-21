using GraphQL.Authorization;
using System.Security.Claims;

namespace PawPadIO.Hub.GraphQL
{
    public class UserContext : IProvideClaimsPrincipal
    {
        public ClaimsPrincipal User { get; set; }
    }
}
