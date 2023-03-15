using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GloiathNationalBank.Common.Http;
using GloiathNationalBank.Storage;

namespace GloiathNationalBank.Services.Clients.Rates
{
    public class RateClient : Client, IRateClient
    {
        /// <summary>
        /// The storage provider
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RateClient"/> class.
        /// </summary>
        /// <param name="storageProvider">The storage provider.</param>
        public RateClient(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        /// <summary>
        /// Gets the rates.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Rates service unavailable.</exception>
        public async Task<List<RateResponse>> GetRates()
        {
            string url = $"{GetBaseUrl()}/rates.json";

            List<RateResponse> result;
            try
            {
                result = await this.Get<List<RateResponse>>(url);
                
                List<RateResponse> value = result;
                _ = Task.Run(async () =>
                {
                    await this.storageProvider.Add(GetKey(), value);
                });
            }
            catch (Exception)
            {
                result = await this.storageProvider.Get<List<RateResponse>>(GetKey());
            }

            if (result == null)
            {
                throw new ApplicationException("Rates service unavailable. ");
            }

            return result;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <returns></returns>
        private static string GetKey()
        {
            return "rates:v1";
        }
    }
}