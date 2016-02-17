using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    public class DealsDB
    {
        private MongoDB.Driver.MongoClient cli;

        public DealsDB(string dbname)
        {
            cli = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
        }

        public void AddDeal(Deal deal)
        {

        }
    }
}