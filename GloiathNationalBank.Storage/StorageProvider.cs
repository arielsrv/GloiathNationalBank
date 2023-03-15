using System.Threading.Tasks;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace GloiathNationalBank.Storage
{
    public class StorageProvider : IStorageProvider
    {
        /// <summary>
        ///     The redis cache client
        /// </summary>
        private readonly IRedisCacheClient redisCacheClient;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StorageProvider" /> class.
        /// </summary>
        /// <param name="redisCacheClient">The redis cache client.</param>
        public StorageProvider(IRedisCacheClient redisCacheClient)
        {
            this.redisCacheClient = redisCacheClient;
        }

        /// <summary>
        ///     Adds the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public async Task Add<T>(string key, T value)
        {
            await redisCacheClient
                .GetDbFromConfiguration()
                .AddAsync(key, value);
        }

        /// <summary>
        ///     Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<T> Get<T>(string key)
        {
            return await redisCacheClient
                .GetDbFromConfiguration()
                .GetAsync<T>(key);
        }
    }
}