using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Api.Models
{
    public class GroupMembership
    {
        public User User { get; set; }
        public Group Group { get; set; }
        public string FromIdP { get; set; }
    }
}
