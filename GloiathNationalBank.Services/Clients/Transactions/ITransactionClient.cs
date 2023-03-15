using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloiathNationalBank.Services.Clients.Transactions
{
    public interface ITransactionClient
    {
        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <returns></returns>
        Task<List<TransactionResponse>> GetTransactions();
    }
}