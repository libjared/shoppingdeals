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
        private readonly IMongoCollection<User> userCollection;
        private string DatabaseName { get; }

        private const string DealsCollectionName = "deals";
        private const string UserCollectionName = "user";

        private Dictionary<string, User> LoggedInUsers { get; }

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

        #region Deals

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
            catch (MongoWriteException whatException)
            {
                if (whatException.Message.Contains("E11000"))
                    throw new ArgumentException("A deal with the same unique information has already been added.");
            }
        }

        #endregion

        #region User

        private async Task CreateUserCollection()
        {
            await db.CreateCollectionAsync(UserCollectionName);
            var keys = Builders<User>.IndexKeys
                .Ascending("Name");
            await userCollection.Indexes.CreateOneAsync(keys, new CreateIndexOptions
            {
                Unique = true
            });
        }

        public async Task RegisterUser(string desiredName, string password)
        {
            //TODO: check dupe name
            var storedHash = PasswordStorage.Hash(password);
            var thisUser = new User(desiredName, storedHash);
            await AddUser(thisUser);
        }

        public async Task<string> LoginUser(string username, string password)
        {
            var thisUser = await GetUserByName(username);
            if (thisUser == null) //user does not exist
            {
                throw new ArgumentException();
            }

            var isOkCredentials = PasswordStorage.PasswordMatch(password, thisUser.PasswordHash);
            if (!isOkCredentials) //bad password
            {
                throw new ArgumentException();
            }

            //login success. generate api key and log him in
            var uniqBytes = PasswordStorage.GenerateRandomBytes(30);
            var apiKey = Convert.ToBase64String(uniqBytes);
            LoggedInUsers.Add(apiKey, thisUser);
            return apiKey;
        }

        private async Task<User> GetUserByName(string username)
        {
            var filter = Builders<User>.Filter.Eq("Name", username);
            var users = await userCollection.FindAsync<User>(filter);
            return users.FirstOrDefault();
        }

        private async Task AddUser(User user)
        {
            try
            {
                await userCollection.InsertOneAsync(user);
            }
            catch (MongoWriteException whatException)
            {
                if (whatException.Message.Contains("E11000"))
                    throw new ArgumentException("A user with the same name has already been added.");
            }
        }

        #endregion
    }
}
