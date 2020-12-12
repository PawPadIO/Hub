using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PawPadIO.Hub.Domain.Data;
using PawPadIO.Hub.Domain.Models;

namespace PawPadIO.Hub.Domain.Services
{
    public class PawPadIOUserService : IUserService<HubUser>
    {
        private readonly HubDbContext _dbContext;

        public PawPadIOUserService(HubDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUserAsync(HubUser user, CancellationToken cancellationToken = default)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (await _dbContext.HubUsers.AnyAsync(u => u.Id == user.Id, cancellationToken))
                throw new DuplicateNameException(nameof(user));

            cancellationToken.ThrowIfCancellationRequested();

            await _dbContext.HubUsers.AddAsync(user, cancellationToken); // Seems silly to be async, but used in some SQL Server scenarios

            await _dbContext.SaveChangesAsync();
        }

        public async Task<HubUser> GetUserFromIssuerAsync(string issuer, string subject, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(issuer))
                throw new ArgumentNullException(nameof(issuer));
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));

            var user = await _dbContext.HubUsers
                .FirstOrDefaultAsync(u => u.Issuer == issuer && u.Subject == subject, cancellationToken);

            return user;
        }
    }
}
