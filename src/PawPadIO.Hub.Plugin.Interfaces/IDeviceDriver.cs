using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Plugin.Interfaces
{
    public interface IDeviceDriver
    {
        Task OnRegisterAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task OnActivateAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task OnDeactivateAsync(CancellationToken cancellationToken = default(CancellationToken));
        //Task Action(Device device, string ActionName, Dictionary<string, string> parameters, CancellationToken cancellationToken = default(CancellationToken));
    }
}
