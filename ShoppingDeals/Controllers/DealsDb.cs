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
            await db.DropCollectionAsync(CollectionName);

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

        public async Task<IEnumerable<Deal>> GetDeals(string prod = null, string store = null, string zip = null)
        {
            var builder = Builders<Deal>.Filter;

            var filterP = prod != null ? builder.Eq("ProductName", prod) : FilterDefinition<Deal>.Empty;
            var filterS = store != null ? builder.Eq("StoreName", store) : FilterDefinition<Deal>.Empty;
            var filterZ = zip != null ? builder.Eq("ZipCode", zip) : FilterDefinition<Deal>.Empty;

            var filterFinal = filterP & filterS & filterZ; 

            var cursor = await dealCollection.FindAsync<Deal>(filterFinal);
            var deals = await cursor.ToListAsync();
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