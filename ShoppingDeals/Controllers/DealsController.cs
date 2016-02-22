using System.Collections.Generic;
using System.Web.Http;
using ShoppingDeals.Models;
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
        public async Task<IEnumerable<Deal>> GetDeals(string prod = null, string store = null, string zip = null)
        {
            return await db.GetDeals(prod, store, zip);
        }
    }
}