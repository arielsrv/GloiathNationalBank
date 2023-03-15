using System.Collections.Generic;

namespace GloiathNationalBank.Services.Transactions
{
    public class BaseTransactionDTO
    {
        /// <summary>
        ///     Gets or sets the sku.
        /// </summary>
        /// <value>
        ///     The sku.
        /// </value>
        public string Sku { get; set; }
    }

    public class TransactionDTO : BaseTransactionDTO
    {
        /// <summary>
        ///     Gets or sets the amount.
        /// </summary>
        /// <value>
        ///     The amount.
        /// </value>
        public double Amount { get; set; }

        /// <summary>
        ///     Gets or sets the currency.
        /// </summary>
        /// <value>
        ///     The currency.
        /// </value>
        public string Currency { get; set; }
    }

    public class SearchTransactionDTO
    {
        /// <summary>
        ///     Gets or sets the transactions.
        /// </summary>
        /// <value>
        ///     The transactions.
        /// </value>
        public List<TransactionDTO> Transactions { get; set; }

        /// <summary>
        ///     Gets or sets the total amount.
        /// </summary>
        /// <value>
        ///     The total amount.
        /// </value>
        public double TotalAmount { get; set; }

        /// <summary>
        ///     Gets the sku.
        /// </summary>
        /// <value>
        ///     The sku.
        /// </value>
        public string SKU { get; internal set; }
    }
}