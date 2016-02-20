using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ShoppingDeals.Models;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/deals")]
    public class DealsController : ApiController
    {
        private static DealsDB db;

        public DealsController()
        {
            if (db == null)
            {
                db = new DealsDB();
                db.Reinitialize();
            }
        }

        [Route("")]
        public async Task<IEnumerable<Deal>> GetDeals()
        {
            return await db.GetDeals();
        }
    }
}