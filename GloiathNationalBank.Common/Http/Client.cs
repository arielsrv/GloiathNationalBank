using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Polly;
using Polly.Bulkhead;

namespace GloiathNationalBank.Common.Http
{
    public abstract class Client
    {
        /// <summary>
        ///     The logger
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     The HTTP client
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        ///     The maximum parallelization
        /// </summary>
        private readonly int maxParallelization;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Client" /> class.
        /// </summary>
        protected Client()
        {
            string baseUrl = GetValue("BaseAddress");
            string timeout = GetValue("Timeout");
            maxParallelization = Convert.ToInt32(GetValue("MaxParallelization"));

            httpClient = new HttpClient
            {
                BaseAddress = !string.IsNullOrEmpty(baseUrl) ? new Uri(baseUrl) : default,
                Timeout = !string.IsNullOrEmpty(timeout)
                    ? TimeSpan.FromMilliseconds(Convert.ToDouble(timeout))
                    : TimeSpan.FromMilliseconds(200)
            };
        }

        /// <summary>
        ///     Gets the configuration prefix.
        /// </summary>
        /// <param name="configKey">The configuration key.</param>
        /// <returns></returns>
        private string GetValue(string configKey)
        {
            return ConfigurationManager.AppSettings.Get($"{GetType().Name}.{configKey}");
        }

        /// <summary>
        ///     Gets the base URL.
        /// </summary>
        /// <returns></returns>
        protected string GetBaseUrl()
        {
            return httpClient.BaseAddress.ToString();
        }

        /// <summary>
        ///     Gets the specified request URI.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUri">The request URI.</param>
        /// <param name="headers">The headers.</param>
        /// <returns></returns>
        protected async Task<T> Get<T>(string requestUri, IDictionary<string, string> headers = null)
        {
            AsyncBulkheadPolicy<T> bulkhead = Policy.BulkheadAsync<T>(maxParallelization, int.MaxValue);

            return await bulkhead.ExecuteAsync(async () =>
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestUri);
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (httpRequestMessage.Content != null && httpRequestMessage.Content.Headers != null)
                    AddHeaders(headers, httpRequestMessage.Content.Headers);

                using (HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage))
                {
                    string response = await httpResponseMessage.Content.ReadAsStringAsync();

                    if (!httpResponseMessage.IsSuccessStatusCode)
                    {
                        logger.Error($"uri: {requestUri}, response: {response}");
                        throw new ApiException(response);
                    }

                    return JsonConvert.DeserializeObject<T>(response);
                }

                ;
            });
        }

        /// <summary>
        ///     Adds the headers.
        /// </summary>
        /// <param name="headers">The headers.</param>
        /// <param name="httpContentHeaders">The HTTP content headers.</param>
        private static void AddHeaders(IDictionary<string, string> headers, HttpContentHeaders httpContentHeaders)
        {
            if (headers != null && headers.Count > 0)
                foreach (KeyValuePair<string, string> item in headers)
                    httpContentHeaders.Add(item.Key, item.Value);
        }
    }
}