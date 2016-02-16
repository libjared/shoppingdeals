using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingDeals.Models;

namespace ShoppingDealsTests
{
    [TestClass]
    public class MongoTest
    {
        [TestMethod]
        public void TestMongoInsert()
        {
            var cli = new MongoDB.Driver.MongoClient("mongodb://localhost:27017");
            var db = cli.GetDatabase("mock");
            var col = db.GetCollection<Deal>("deals");

            var mod = new Deal()
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

            col.InsertOneAsync(mod).Wait();
        }
    }
}
