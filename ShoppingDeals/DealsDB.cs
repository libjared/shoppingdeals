using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingDeals.Controllers
{
    public class DealsDB
    {
        private MongoDB.Driver.MongoClient cli;

        public DealsDB(string dbname)
        {
            cli = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
        }
    }
}