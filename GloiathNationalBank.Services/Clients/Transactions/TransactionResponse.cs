using GloiathNationalBank.Services.Rates.Currencies;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace GloiathNationalBank.Services.Clients.Transactions
{
    public class TransactionResponse
    {
        /// <summary>
        ///     Gets or sets the sku.
        /// </summary>
        /// <value>
        ///     The sku.
        /// </value>
        [JsonProperty("sku")]
        public string Sku { get; set; }

        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>
        ///     The amount.
        /// </value>
        [JsonProperty("amount")]
        public double Amount { get; set; }

        /// <summary>
        ///     Gets or sets the currency.
        /// </summary>
        /// <value>
        ///     The currency.
        /// </value>
        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public Currency Currency { get; set; }
    }
}