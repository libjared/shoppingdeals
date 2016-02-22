using System.Collections.Generic;
using System.Web.Http;
using ShoppingDeals.Models;
using System.Threading.Tasks;

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
        public async Task<IEnumerable<Deal>> GetDeals()
        {
            return await db.GetDeals();
        }
    }
}