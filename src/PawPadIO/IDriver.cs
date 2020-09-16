using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PawPadIO
{
    public interface IDriver : IDisposable
    {
        string InstanceId { get; }

        bool IsRunning { get; }

        Task InitialiseAsync(IValueStore valueStore, CancellationToken cancellationToken = default);

        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync(CancellationToken cancellationToken = default);

        [DriverAction(Name ="devices")]
        Task<IEnumerable<Device>> GetDevicesAsync(CancellationToken cancellationToken = default);
    }
}
