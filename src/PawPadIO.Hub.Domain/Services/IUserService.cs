using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PawPadIO.Hub.Domain.Models;

namespace PawPadIO.Hub.Domain.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> ListUsersAsync(CancellationToken cancellationToken = default);
        Task<User> GetCurrentUserAsync(CancellationToken cancellationToken = default);
        Task<User> GetUserFromIssuerAsync(string issuer, string id, CancellationToken cancellationToken = default);
        Task<User> GetUserAsync(int id, CancellationToken cancellationToken = default);
        Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);
    }
}
