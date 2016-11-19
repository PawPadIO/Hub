using System;
using System.Threading;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Plugin.Interfaces.LightBulb
{
    public interface ILightBulbDriver : IDeviceDriver
    {
        Task TurnOn(Guid actionId = new Guid(), CancellationToken cancellationToken = default(CancellationToken));
        Task TurnOff(CancellationToken cancellationToken = default(CancellationToken));
    }
}
