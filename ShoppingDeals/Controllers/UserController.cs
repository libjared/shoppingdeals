using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json.Linq;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/user")]
    public class UserController : ApiController
    {
        private static DealsDb db;

        public static void Initialize()
        {
            StaticDealsDb.Initialize("shoppingdeals");
            db = StaticDealsDb.Db;
        }

        [Route("login")]
        [HttpPost]
        public async Task<HttpResponseMessage> Login([FromBody]JToken jsonbody)
        {
            throw new NotImplementedException();
        }
    }
}