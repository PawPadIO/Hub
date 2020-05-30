using System.ComponentModel;

namespace PawPadIO.Hub.Domain.Models
{
    [Description("The residency or guest status of the user. Determines what functions the user can perform.")]
    public enum PrivilegeLevel
    {
        [Description("This user has been manually added as a resident by another resident of the flat. Can perform all functions.")]
        Resident = 3,
        [Description("This user has been physically verified by being present at the flat and scanning a code.")]
        VerifiedGuest = 2,
        [Description("This user has signed up for an account, but has not been physically verified. Can perform a limited number of functions, specifically nothing that will reveal the physical location of the flat without explicit action from a resident.")]
        Guest = 1,
        [Description("This user has been banned, and cannot perform any function.")]
        Banned = 0,
    }
}
