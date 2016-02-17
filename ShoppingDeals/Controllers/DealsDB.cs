using System;
using System.Collections.Generic;
using System.Linq;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    public class DealsDB
    {
        private MongoDB.Driver.MongoClient cli;
        private MongoDB.Driver.IMongoDatabase db;

        public DealsDB()
        {
            cli = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
            db = cli.GetDatabase("shoppingdeals");
        }

        public async void AddDeal(Deal deal)
        {
            var collection = db.GetCollection<Deal>("deals");
            await collection.InsertOneAsync(deal);
        }
    }
}