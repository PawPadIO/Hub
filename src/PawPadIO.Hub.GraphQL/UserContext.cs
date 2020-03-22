using System.Collections.Generic;
using System.Security.Claims;

namespace PawPadIO.Hub.GraphQL
{
    public class UserContext : Dictionary<string, object>
    {
        public ClaimsPrincipal User { get; set; }
    }
}
