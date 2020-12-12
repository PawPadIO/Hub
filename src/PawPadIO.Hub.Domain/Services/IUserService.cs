using System;
using System.Threading;
using System.Threading.Tasks;

namespace PawPadIO.Hub.Domain.Services
{
    public interface IUserService<T>
    {
        Task<T> GetUserFromIssuerAsync(string issuer, string subject, CancellationToken cancellationToken = default);
        Task CreateUserAsync(T user, CancellationToken cancellationToken = default);
    }
}
