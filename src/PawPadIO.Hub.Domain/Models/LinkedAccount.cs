using System;
namespace PawPadIO.Hub.Domain.Models
{
    public class LinkedAccount
    {
        public int Id { get; set; }

        public string Issuer { get; set; }

        public string Subject { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
