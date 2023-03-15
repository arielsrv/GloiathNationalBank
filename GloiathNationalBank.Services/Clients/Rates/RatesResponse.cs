using GloiathNationalBank.Services.Rates.Currencies;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GloiathNationalBank.Services.Clients.Rates
{
    public class RateResponse
    {
        /// <summary>
        ///     Gets or sets from.
        /// </summary>
        /// <value>
        ///     From.
        /// </value>
        [JsonProperty("from")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency From { get; set; }

        /// <summary>
        ///     Gets or sets to.
        /// </summary>
        /// <value>
        ///     To.
        /// </value>
        [JsonProperty("to")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency To { get; set; }

        /// <summary>
        ///     Gets or sets the rate.
        /// </summary>
        /// <value>
        ///     The rate.
        /// </value>
        [JsonProperty("rate")]
        public double Rate { get; set; }
    }
}