using System;
using Microsoft.EntityFrameworkCore;
using PawPadIO.Hub.Domain.Models;

namespace PawPadIO.Hub.Domain.Data
{
    public class HubDbContext : DbContext
    {
        public HubDbContext(DbContextOptions<HubDbContext> options)
            : base(options)
        { }

        public DbSet<HubUser> HubUsers { get; set; }
    }
}
