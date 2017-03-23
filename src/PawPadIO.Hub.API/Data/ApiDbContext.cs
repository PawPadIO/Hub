using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PawPadIO.Hub.Api.Models;

namespace PawPadIO.Hub.Api.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<Device> Devices { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GlobalOperation> GlobalOperations { get; set; }
        public DbSet<UserDevicePermission> UserDevicePermissions { get; set; }
        public DbSet<GroupDevicePermission> GroupDevicePermissions { get; set; }
        public DbSet<UserGlobalPermission> UserGlobalPermissions { get; set; }
        public DbSet<GroupGlobalPermission> GroupGlobalPermissions { get; set; }
    }
}
