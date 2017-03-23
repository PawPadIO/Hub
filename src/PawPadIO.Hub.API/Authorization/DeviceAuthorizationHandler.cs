using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using PawPadIO.Hub.Api.Models;
using PawPadIO.Hub.Api.Services;

namespace PawPadIO.Hub.Api.Authorization
{
    public class DeviceAuthorizationHandler : AuthorizationHandler<DeviceOperationLevelRequirement, Device>
    {
        public DeviceAuthorizationHandler(IPermissionService permissionService)
        {

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DeviceOperationLevelRequirement requirement, Device resource)
        {
            throw new NotImplementedException();
        }
    }
}
