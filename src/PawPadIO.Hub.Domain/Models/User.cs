using System;
using System.Collections.Generic;

namespace PawPadIO.Hub.Domain.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public PrivilegeLevel PrivilegeLevel { get; set; }
        public DateTimeOffset? LastSeen { get; set; }

        public virtual ICollection<LinkedAccount> LinkedAccounts { get; set; }
    }
}
