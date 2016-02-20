using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingDeals.Models;
using MongoDB.Driver;

namespace ShoppingDeals.Controllers
{
    public class DealsDB
    {
        private MongoClient cli;
        private IMongoDatabase db;

        public DealsDB()
        {
            cli = new MongoClient("mongodb://localhost:27017");
            db = cli.GetDatabase("shoppingdeals");
        }

        public void Reinitialize()
        {
            //clear
            db.DropCollection("deals");

            //add a new test document
            Deal testDeal = new Deal()
            {
                Username = "Jared",
                ProductName = "Nintendo 3DS",
                Price = 50.00m,
                StoreName = "Amazon",
                ZipCode = 1234,
                ExpirationDate = DateTime.Now.AddYears(1),
                Likes = 234,
                Dislikes = 1,
            };
            var collection = db.GetCollection<Deal>("deals");
            collection.InsertOne(testDeal);
        }

        public async Task<IEnumerable<Deal>> GetDeals()
        {
            var collection = db.GetCollection<Deal>("deals");
            var c1 = await collection.FindAsync<Deal>(FilterDefinition<Deal>.Empty);
            var deals = await c1.ToListAsync();
            return deals;
        }
    }
}