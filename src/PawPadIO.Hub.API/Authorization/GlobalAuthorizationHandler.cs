using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Api.Authorization
{
    public class GlobalAuthorizationHandler : AuthorizationHandler<GlobalPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GlobalPermissionRequirement requirement)
        {
            throw new NotImplementedException();
        }
    }
}
