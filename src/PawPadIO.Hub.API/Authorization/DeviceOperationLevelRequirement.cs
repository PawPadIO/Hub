using Microsoft.AspNetCore.Authorization;
using PawPadIO.Hub.Api.Models;

namespace PawPadIO.Hub.Api.Authorization
{
    public class DeviceOperationLevelRequirement : IAuthorizationRequirement
    {
        private readonly DevicePermissionLevel _levelRequired;

        public DevicePermissionLevel LevelRequired
        {
            get { return _levelRequired; }
        }
        
        public DeviceOperationLevelRequirement(DevicePermissionLevel levelRequired)
        {
            _levelRequired = levelRequired;
        }
    }
}
