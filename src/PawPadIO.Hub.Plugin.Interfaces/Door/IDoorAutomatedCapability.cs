using System.Threading;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Plugin.Interfaces.Door
{
    public delegate Task DoorStateReceived();

    public interface IDoorAutomatedCapability : IDoorDriver
    {
        DoorStateReceived DoorStateRecieved { get; set; }
        Task OpenDoor(CancellationToken cancellationToken = default(CancellationToken));
        Task CloseDoor(CancellationToken cancellationToken = default(CancellationToken));
        Task RequestDoorStatus(CancellationToken cancellationToken = default(CancellationToken));
    }
}
