using System;
namespace PawPadIO.Hub.Domain.Models
{
    public class HubUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Issuer { get; set; }

        public string Subject { get; set; }
    }
}
