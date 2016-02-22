﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingDeals.Models;
using MongoDB.Driver;

namespace ShoppingDeals.Controllers
{
    public class DealsDb
    {
        private IMongoDatabase db;

        public DealsDb()
        {
            var cli = new MongoClient("mongodb://localhost:27017");
            db = cli.GetDatabase("shoppingdeals");
        }

        public void Reinitialize()
        {
            db.DropCollection("deals");

            CreateDealsCollection();
        }

        private void CreateDealsCollection()
        {
            db.CreateCollection("deals");
            var collection = db.GetCollection<Deal>("deals");
            var keys = Builders<Deal>.IndexKeys
                .Ascending("StoreName").Ascending("ProductName")
                .Ascending("ExpirationDate").Ascending("Price");
            collection.Indexes.CreateOne(keys, new CreateIndexOptions
            {
                Unique = true
            });
        }

        public async Task<IEnumerable<Deal>> GetDeals()
        {
            var collection = db.GetCollection<Deal>("deals");
            var c1 = await collection.FindAsync<Deal>(FilterDefinition<Deal>.Empty);
            var deals = await c1.ToListAsync();
            return deals;
        }

        public async Task AddDeal(Deal deal)
        {
            var collection = db.GetCollection<Deal>("deals");
            try
            {
                await collection.InsertOneAsync(deal);
            }
            catch (MongoWriteException whatException)
            {
                if (whatException.Message.Contains("E11000"))
                    throw new ArgumentException("A deal with the same key has already been added.");
                throw;
            }

            System.Diagnostics.Debug.WriteLine(collection.CountAsync(FilterDefinition<Deal>.Empty));
        }
    }
}