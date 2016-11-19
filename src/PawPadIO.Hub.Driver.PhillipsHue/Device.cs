using PawPadIO.Hub.Plugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using PawPadIO.Hub.Plugin.Interfaces.LightBulb;

namespace PawPadIO.Hub.Driver.PhillipsHue
{
    public class Device : 
        ILightBulbDriver,
        ILightBulbBrightnessCapability, ILightBulbHueCapability
    {
        public Device()
        {
        }

        public Task OnActivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task OnDeactivateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task OnRegisterAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TurnOff(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TurnOn(Guid actionId = default(Guid), CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
