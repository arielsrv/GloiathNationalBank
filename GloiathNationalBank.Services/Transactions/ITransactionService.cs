using System.Collections.Generic;
using System.Threading.Tasks;

namespace GloiathNationalBank.Services.Transactions
{
    public interface ITransactionService
    {
        Task<List<TransactionDTO>> GetTransactions();

        Task<SearchTransactionDTO> GetTransactions(string sku);
    }
}