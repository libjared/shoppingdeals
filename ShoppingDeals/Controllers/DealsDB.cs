using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingDeals.Models;
using MongoDB.Driver;

namespace ShoppingDeals.Controllers
{
    public class DealsDB : IDealsDB
    {
        private MongoClient cli;
        private IMongoDatabase db;

        public DealsDB()
        {
            cli = new MongoClient("mongodb://localhost:27017");
            db = cli.GetDatabase("shoppingdeals");
        }

        public async void AddDealAsync(Deal deal)
        {
            var collection = db.GetCollection<Deal>("deals");
            await collection.InsertOneAsync(deal);
        }

        public async Task<IEnumerable<Deal>> GetDealsAsync()
        {
            throw new NotImplementedException();
        }
    }
}