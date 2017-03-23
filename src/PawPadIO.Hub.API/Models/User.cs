using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<UserDevicePermission> UserDevicePermissions { get; set; }
        public ICollection<UserGlobalPermission> UserGlobalPermissions { get; set; }
    }
}
