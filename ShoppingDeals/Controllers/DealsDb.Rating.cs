using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    public partial class DealsDb
    {
        private const string RatingCollectionName = "rating";
        private readonly IMongoCollection<Rating> ratingCollection;

        private async Task CreateRatingCollection()
        {
            await db.CreateCollectionAsync(RatingCollectionName);
            var keys = Builders<Rating>.IndexKeys
                .Ascending("AboutDeal").Ascending("AboutUser");
            await ratingCollection.Indexes.CreateOneAsync(keys, new CreateIndexOptions
            {
                Unique = true
            });

            var expireKeys = Builders<Rating>.IndexKeys
                .Ascending("ExpirationDate");
            await ratingCollection.Indexes.CreateOneAsync(expireKeys, new CreateIndexOptions<Rating>
            {
                ExpireAfter = new TimeSpan(0L) //0 means interpret key as date
            });
        }

        public async Task RateDeal(Deal whatDeal, User asUser, bool isPositive)
        {
            //find a rating if it already exists, if not, make a new rating
            var filterD = Builders<Rating>.Filter.Eq("AboutDeal", whatDeal.Id);
            var filterU = Builders<Rating>.Filter.Eq("AboutUser", asUser.Id);
            var filterFinal = filterD & filterU;

            var updateDef = Builders<Rating>.Update
                .Set("AboutDeal", whatDeal.Id)
                .Set("AboutUser", asUser.Id)
                .Set("IsPositive", isPositive)
                .Set("ExpirationDate", whatDeal.ExpirationDate);
            await ratingCollection.UpdateOneAsync(filterFinal, updateDef, new UpdateOptions { IsUpsert = true });
        }

        public async Task<int> GetRating(Deal deal)
        {
            //find all the ratings that match this deal
            var filterD = Builders<Rating>.Filter.Eq("AboutDeal", deal.Id);
            var cursor = await ratingCollection.FindAsync<Rating>(filterD);
            var ratings = await cursor.ToListAsync();

            return ratings.Sum(r => r.IsPositive ? 1 : -1);
        }
    }
}