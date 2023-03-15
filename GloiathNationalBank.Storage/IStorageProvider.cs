using System.Threading.Tasks;

namespace GloiathNationalBank.Storage
{
    public interface IStorageProvider
    {
        /// <summary>
        ///     Gets the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<T> Get<T>(string key);

        /// <summary>
        ///     Adds the specified key.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        Task Add<T>(string key, T value);
    }
}