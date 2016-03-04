using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    public partial class DealsDb
    {
        private const string DealsCollectionName = "deals";
        private readonly IMongoCollection<Deal> dealCollection;

        private async Task CreateDealsCollection()
        {
            await db.CreateCollectionAsync(DealsCollectionName);
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
            catch (MongoWriteException ex)
            {
                if (ex.Message.Contains("E11000"))
                {
                    throw new AlreadyExistsException("A deal with the same unique information has already been added.", ex);
                }
            }
        }
    }
}