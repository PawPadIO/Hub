using PawPadIO.Hub.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Api.Services
{
    public class PermissionService : IPermissionService
    {
        #region Generic Context Permission Check
        public async Task<DevicePermissionLevel> GetDevicePermissionAsync(int user, List<string> groups, int device, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<GlobalPermissionLevel> GetGlobalPermissionAsync(int user, List<string> groups, string setting, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        #endregion

        #region User/Group Specific Permission Check
        public async Task<DevicePermissionLevel> GetDevicePermissionForGroupAsync(string group, int device, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<DevicePermissionLevel> GetDevicePermissionForUserAsync(int user, int device, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<GlobalPermissionLevel> GetGlobalPermissionForGroupAsync(string group, string setting, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task<GlobalPermissionLevel> GetGlobalPermissionForUserAsync(int user, string setting, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Set Permission
        public async Task SetDevicePermissionForGroupAsync(string group, int device, DevicePermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task SetDevicePermissionForUserAsync(int user, int device, DevicePermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task SetGlobalPermissionForGroupAsync(string group, string setting, GlobalPermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public async Task SetGlobalPermissionForUserAsync(int user, string setting, GlobalPermissionLevel permissionLevel, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
