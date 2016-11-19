using PawPadIO.Hub.Plugin.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using PawPadIO.Hub.Plugin.Interfaces.Door;
using PawPadIO.Hub.Plugin.Interfaces.LightBulb;

namespace PawPadIO.Hub.Driver.TequinGarageDoor
{
    public class Device
        : IDoorDriver, ILightBulbDriver,
        IDoorAutomatedCapability
    {
        public Device()
        {
        }

        public DoorStateReceived DoorStateRecieved { get; set; }

        public void ExampleSomethingGotRecieved()
        {
            // Message came in! Yay!
            if (DoorStateRecieved == null) return;
            DoorStateRecieved();
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
