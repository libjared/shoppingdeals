using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ShoppingDeals.Models;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/deals")]
    public class DealsController : ApiController
    {
        private static DealsDb db;

        public static void Initialize()
        {
            StaticDealsDb.Initialize("shoppingdeals");
            db = StaticDealsDb.Db;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDeals(string prod = null, string store = null, string zip = null)
        {
            var data = await db.GetDeals(prod, store, zip);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostDeal(PostedDeal postedDeal)
        {
            await db.AddDeal(postedDeal.ToDeal());
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}