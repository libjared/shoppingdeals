using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingDeals.Models;
using MongoDB.Driver;

namespace ShoppingDeals.Controllers
{
    public class DealsDb
    {
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<Deal> dealCollection;
        private string CollectionName { get; }

        public DealsDb(string collectionName)
        {
            CollectionName = collectionName;

            var cli = new MongoClient("mongodb://localhost:27017");
            db = cli.GetDatabase("shoppingdeals");
            dealCollection = db.GetCollection<Deal>(collectionName);
        }

        public async Task Reinitialize()
        {
            db.DropCollection(CollectionName);

            await CreateDealsCollection();
        }

        private async Task CreateDealsCollection()
        {
            await db.CreateCollectionAsync(CollectionName);
            var keys = Builders<Deal>.IndexKeys
                .Ascending("StoreName").Ascending("ProductName")
                .Ascending("ExpirationDate").Ascending("Price");
            await dealCollection.Indexes.CreateOneAsync(keys, new CreateIndexOptions
            {
                Unique = true
            });
        }

        public async Task<IEnumerable<Deal>> GetDeals()
        {
            var c1 = await dealCollection.FindAsync<Deal>(FilterDefinition<Deal>.Empty);
            var deals = await c1.ToListAsync();
            return deals;
        }

        public async Task AddDeal(Deal deal)
        {
            try
            {
                await dealCollection.InsertOneAsync(deal);
            }
            catch (MongoWriteException whatException)
            {
                if (whatException.Message.Contains("E11000"))
                    throw new ArgumentException("A deal with the same key has already been added.");
                throw;
            }
        }
    }
}