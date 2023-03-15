using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloiathNationalBank.Services.Clients.Rates
{
    public interface IRateClient
    {
        /// <summary>
        /// Gets the rates.
        /// </summary>
        /// <returns></returns>
        Task<List<RateResponse>> GetRates();
    }
}