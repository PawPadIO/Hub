using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PawPadIO
{
    public interface IValueStore
    {
        Task<TType> GetValueAsync<TType>(string key, CancellationToken cancellationToken = default);
        Task<object> GetValueAsync(string key, CancellationToken cancellationToken = default);
        Task StoreValueAsync(string key, object value, CancellationToken cancellationToken = default);
    }
}
