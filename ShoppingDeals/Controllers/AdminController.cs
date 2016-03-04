using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/admin")]
    public class AdminController : ApiController
    {
        [Route("resetdb")]
        [HttpGet]
        public async Task<HttpResponseMessage> ResetDb()
        {
            await StaticDealsDb.Db.Reinitialize();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}