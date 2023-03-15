using System.Threading.Tasks;
using System.Web.Http;
using GloiathNationalBank.Services.Rates;

namespace GloiathNationalBank.WebApi.Controllers
{
    [RoutePrefix("api/v1/rates")]
    public class RateController : BaseController
    {
        /// <summary>
        ///     The rate service
        /// </summary>
        private readonly IRateService rateService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RateController" /> class.
        /// </summary>
        /// <param name="rateService">The rate service.</param>
        public RateController(IRateService rateService)
        {
            this.rateService = rateService;
        }

        /// <summary>
        ///     Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            return Execute(await rateService.GetRates());
        }
    }
}