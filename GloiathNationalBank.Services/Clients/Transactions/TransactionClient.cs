using System.Collections.Generic;
using System.Threading.Tasks;
using GloiathNationalBank.Common.Http;

namespace GloiathNationalBank.Services.Clients.Transactions
{
    public class TransactionClient : Client, ITransactionClient
    {
        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TransactionResponse>> GetTransactions()
        {
            string url = $"{GetBaseUrl()}/transactions.json";
            return await this.Get<List<TransactionResponse>>(url);
        }
    }
}