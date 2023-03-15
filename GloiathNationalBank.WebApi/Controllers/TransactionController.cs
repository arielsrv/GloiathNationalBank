using System.Threading.Tasks;
using System.Web.Http;
using GloiathNationalBank.Services.Transactions;

namespace GloiathNationalBank.WebApi.Controllers
{
    [RoutePrefix("api/v1/transactions")]
    public class TransactionController : BaseController
    {
        /// <summary>
        ///     The transaction service
        /// </summary>
        private readonly ITransactionService transactionService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionController" /> class.
        /// </summary>
        /// <param name="transactionService">The transaction service.</param>
        public TransactionController(ITransactionService transactionService)
        {
            this.transactionService = transactionService;
        }

        /// <summary>
        ///     Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            return Execute(await transactionService.GetTransactions());
        }

        /// <summary>
        ///     Gets the specified sku.
        /// </summary>
        /// <param name="sku">The sku.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<IHttpActionResult> Get([FromUri] string sku)
        {
            return Execute(await transactionService.GetTransactions(sku));
        }
    }
}