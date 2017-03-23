using PawPadIO.Hub.Api.Services;

namespace PawPadIO.Hub.Api.Models
{
    public class UserDevicePermission
    {
        public User User { get; set; }
        public Device Device { get; set; }
        public DevicePermissionLevel PermissionLevel { get; set; } 
    }

    public class GroupDevicePermission
    {
        public Group Group { get; set; }
        public Device Device { get; set; }
        public DevicePermissionLevel PermissionLevel { get; set; }
    }
}
