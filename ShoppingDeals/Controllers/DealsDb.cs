using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingDeals.Models;
using MongoDB.Driver;

namespace ShoppingDeals.Controllers
{
    public partial class DealsDb
    {
        private readonly IMongoDatabase db;
        private string DatabaseName { get; }

        public DealsDb(string databaseName)
        {
            DatabaseName = databaseName;

            var cli = new MongoClient("mongodb://localhost:27017");
            db = cli.GetDatabase(databaseName);
            dealCollection = db.GetCollection<Deal>(DealsCollectionName);
            userCollection = db.GetCollection<User>(UserCollectionName);
            LoggedInUsers = new Dictionary<string, User>();
        }

        public async Task Reinitialize()
        {
            await db.DropCollectionAsync(DealsCollectionName);
            await db.DropCollectionAsync(UserCollectionName);

            await CreateDealsCollection();
            await CreateUserCollection();
        }
    }
}
