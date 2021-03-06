﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    public partial class DealsDb
    {
        private const string UserCollectionName = "user";
        private readonly IMongoCollection<User> userCollection;
        private Dictionary<Guid, User> LoggedInUsers { get; }

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
            var storedHash = PasswordStorage.Hash(password);
            var thisUser = new User(desiredName, storedHash);
            await AddUser(thisUser);
        }

        public async Task<Guid> LoginUser(string username, string password)
        {
            var thisUser = await GetUserByName(username);
            if (thisUser == null) //user does not exist
            {
                throw new CredentialsException();
            }

            var isOkCredentials = PasswordStorage.PasswordMatch(password, thisUser.PasswordHash);
            if (!isOkCredentials) //bad password
            {
                throw new CredentialsException();
            }

            //login success. generate api key and log him in
            var apiKey = Guid.NewGuid();
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
            catch (MongoWriteException ex)
            {
                if (ex.Message.Contains("E11000"))
                {
                    throw new AlreadyExistsException("A user with the same name has already been added.", ex);
                }
            }
        }

        public User GetUserFromApiKey(Guid apikey)
        {
            User thisUser;
            LoggedInUsers.TryGetValue(apikey, out thisUser);
            return thisUser;
        }
    }
}