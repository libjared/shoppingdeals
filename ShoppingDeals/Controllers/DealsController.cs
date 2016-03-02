using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/deals")]
    public class DealsController : ApiController
    {
        private static DealsDb db;

        public static async Task Initialize()
        {
            db = new DealsDb("deals");
            await db.Reinitialize();
        }

        [Route("")]
        public async Task<HttpResponseMessage> GetDeals(string prod = null, string store = null, string zip = null)
        {
            var data = await db.GetDeals(prod, store, zip);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
    }
}