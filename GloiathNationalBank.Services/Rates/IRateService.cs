using GloiathNationalBank.Services.Rates.Currencies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloiathNationalBank.Services.Rates
{
    public interface IRateService
    {
        /// <summary>
        /// Gets the rates.
        /// </summary>
        /// <returns></returns>
        Task<List<RateDto>> GetRates();

        /// <summary>
        /// Gets the rate.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        Task<double> GetRate(Currency source, Currency target);
    }
}