using GloiathNationalBank.Services.Clients;
using GloiathNationalBank.Services.Rates;
using GloiathNationalBank.Services.Rates.Currencies;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GloiathNationalBank.Services.Clients.Transactions;

namespace GloiathNationalBank.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        /// <summary>
        /// The transaction client
        /// </summary>
        private readonly ITransactionClient transactionClient;

        /// <summary>
        /// The rate service
        /// </summary>
        private readonly IRateService rateService;

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionService"/> class.
        /// </summary>
        /// <param name="transactionClient">The transaction client.</param>
        /// <param name="rateService">The rate service.</param>
        public TransactionService(ITransactionClient transactionClient, IRateService rateService)
        {
            this.transactionClient = transactionClient;
            this.rateService = rateService;
        }

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <returns></returns>
        public async Task<List<TransactionDTO>> GetTransactions()
        {
            List<TransactionResponse> response = await transactionClient.GetTransactions();

            List<TransactionDTO> result = new List<TransactionDTO>();

            if (response != null)
            {
                foreach (TransactionResponse transactionResponse in response)
                {
                    TransactionDTO rateDto = new TransactionDTO
                    {
                        Sku = transactionResponse.Sku,
                        Amount = transactionResponse.Amount,
                        Currency = transactionResponse.Currency.ToString()
                    };

                    result.Add(rateDto);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <param name="sku">The sku.</param>
        /// <returns></returns>
        public async Task<SearchTransactionDTO> GetTransactions(string sku)
        {
            try
            {
                List<TransactionResponse> response = await transactionClient.GetTransactions();

                SearchTransactionDTO result = new SearchTransactionDTO
                {
                    Transactions = new List<TransactionDTO>(),
                    SKU = sku
                };

                if (response != null)
                {
                    IEnumerable<TransactionResponse> filtered = response.Where(transaction => transaction.Sku == sku);

                    if (filtered.Any())
                    {
                        foreach (TransactionResponse transactionResponse in filtered)
                        {
                            TransactionDTO transactionDTO = new TransactionDTO
                            {
                                Sku = transactionResponse.Sku,
                                Amount = await ConvertAmount(transactionResponse.Amount, transactionResponse.Currency, Currency.EUR),
                                Currency = Currency.EUR.ToString()
                            };

                            result.Transactions.Add(transactionDTO);
                        }

                        result.TotalAmount = result
                            .Transactions
                            .Sum(transaction => transaction.Amount);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                logger.Error($"SKU: {sku}, Error: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Converts the amount.
        /// </summary>
        /// <param name="amount">The amount.</param>
        /// <param name="originalCurrency">The original currency.</param>
        /// <param name="targetCurrency">The target currency.</param>
        /// <returns></returns>
        private async Task<double> ConvertAmount(double amount, Currency originalCurrency, Currency targetCurrency)
        {
            double rate = await this.rateService.GetRate(originalCurrency, targetCurrency);
            return Math.Round(amount * rate, 2, MidpointRounding.ToEven);
        }
    }
}