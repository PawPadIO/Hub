using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using PawPadIO.Hub.Api.Models;

namespace PawPadIO.Hub.Api.Services
{
    public interface IPermissionService
    {
        Task<DevicePermissionLevel> GetDevicePermissionAsync(int user, List<string> groups, int device, CancellationToken cancellationToken = default(CancellationToken));
        Task<GlobalPermissionLevel> GetGlobalPermissionAsync(int user, List<string> groups,string setting, CancellationToken cancellationToken = default(CancellationToken));
        Task<DevicePermissionLevel> GetDevicePermissionForUserAsync(int user, int device, CancellationToken cancellationToken = default(CancellationToken));
        Task<GlobalPermissionLevel> GetGlobalPermissionForUserAsync(int user, string setting, CancellationToken cancellationToken = default(CancellationToken));
        Task<DevicePermissionLevel> GetDevicePermissionForGroupAsync(string group, int device, CancellationToken cancellationToken = default(CancellationToken));
        Task<GlobalPermissionLevel> GetGlobalPermissionForGroupAsync(string group, string setting, CancellationToken cancellationToken = default(CancellationToken));
        Task SetDevicePermissionForUserAsync(int user, int device, DevicePermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken));
        Task SetGlobalPermissionForUserAsync(int user, string setting, GlobalPermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken));
        Task SetDevicePermissionForGroupAsync(string group, int device, DevicePermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken));
        Task SetGlobalPermissionForGroupAsync(string group, string setting, GlobalPermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken));
    }
}
