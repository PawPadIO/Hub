using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PawPadIO
{
    [Description("The open/closed status of the door.")]
    public enum DoorPosition
    {
        [Description("Door is fully closed")]
        PartiallyOpen = 0,
        [Description("Door is partially open")]
        Closed = -1,
        [Description("Door is fully open")]
        Open = 1,
    }
}