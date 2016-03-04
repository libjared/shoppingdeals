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

        public static async Task Initialize()
        {
            StaticDealsDb.Initialize("shoppingdeals");
            db = StaticDealsDb.Db;

            await db.Reinitialize();
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
        public async Task<HttpResponseMessage> PostDeal([FromBody]JToken jsonbody)
        {
            if (!IsValidPostedDeal(jsonbody))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            var postedDeal = jsonbody.ToObject<PostedDeal>();

            await db.AddDeal(postedDeal.ToDeal());
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private static bool IsValidPostedDeal(JToken token)
        {
            return HasKeys(token, new List<string>
            {
                "ProductName", "StoreName", "Price", "ExpirationDate", "ZipCode"
            });
        }

        private static bool HasKeys(JToken token, List<string> keys)
        {
            return keys.All(k => token[k] != null);
        }
    }
}