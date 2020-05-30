using System;
using Microsoft.EntityFrameworkCore;

namespace PawPadIO.Hub.Domain.DAL
{
    public static class PawPadIODbContextInitialiser
    {
        public static void Initialise(PawPadIODbContext context)
        {
            context.Database.Migrate();

            // TODO: Seed data, if necessary
        }
    }
}
