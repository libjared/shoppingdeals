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

            IMongoCollection<Deal> collection = CreateCollection();

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

            collection.InsertOne(testDeal);
        }

        private IMongoCollection<Deal> CreateCollection()
        {
            db.CreateCollection("deals");
            var collection = db.GetCollection<Deal>("deals");
            var keys = Builders<Deal>.IndexKeys
                .Ascending("StoreName").Ascending("ProductName")
                .Ascending("ExpirationDate").Ascending("Price");
            collection.Indexes.CreateOne(keys);
            return collection;
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