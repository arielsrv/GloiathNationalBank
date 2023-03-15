using System.Web.Http;

namespace GloiathNationalBank.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        protected IHttpActionResult Execute<T>(T result)
        {
            if (result == null) return NotFound();

            return Ok(result);
        }
    }
}