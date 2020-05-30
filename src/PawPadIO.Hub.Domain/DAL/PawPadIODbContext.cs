using Microsoft.EntityFrameworkCore;
using PawPadIO.Hub.Domain.Models;

namespace PawPadIO.Hub.Domain.DAL
{
    public class PawPadIODbContext : DbContext
    { 
        public PawPadIODbContext(DbContextOptions<PawPadIODbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
