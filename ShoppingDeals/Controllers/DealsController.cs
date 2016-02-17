using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    public class DealsController : ApiController
    {
        private DealsDB db;

        public DealsController()
        {
            db = new DealsDB("shoppingdeals");
        }
    }
}