using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/deals")]
    public class DealsController : ApiController
    {
        private DealsDB db;

        public DealsController()
        {
            db = new DealsDB();
        }

        [Route("")]
        public IEnumerable<Deal> GetDeals()
        {
            return new Deal[] {
                new Deal() {
                    Username = "Jared",
                    ProductName = "Nintendo 3DS",
                    Price = 50.00m,
                    StoreName = "Amazon",
                    ZipCode = 1234,
                    ExpirationDate = DateTime.Now.AddYears(1),
                    Likes = 234,
                    Dislikes = 1,
                }
            };
        }
    }
}