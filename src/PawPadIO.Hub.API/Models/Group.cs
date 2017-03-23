using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Api.Models
{
    public class Group
    {
        public string Id { get; set; }
        public ICollection<GroupDevicePermission> GroupDevicePermissions { get; set; }
        public ICollection<GroupGlobalPermission> GroupGlobalPermissions { get; set; }
    }
}
