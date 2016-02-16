using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
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