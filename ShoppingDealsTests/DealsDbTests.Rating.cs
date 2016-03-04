using System;
using System.Threading.Tasks;
using NUnit.Framework;
using ShoppingDeals.Models;

namespace ShoppingDealsTests
{
    public partial class DealsDbTests
    {
        [Test]
        public async Task RatePositive()
        {
            //register
            await RegUserTestA();
            //login
            var apikey = await LoginUserTestA();
            //post deal
            var testDeal = new Deal
            {
                ProductName = "Nintendo 3DS",
                Price = 50.00m,
                StoreName = "Amazon",
                ZipCode = "1234",
                ExpirationDate = new DateTime(2016, 5, 18, 6, 32, 0)
            };
            var testUser = db.GetUserFromApiKey(apikey);
            testDeal.SetPostedBy(testUser);
            await db.AddDeal(testDeal);
            //like the deal
            await db.RateDeal(testDeal, testUser, true);
            //make sure it's still liked
            var rating = await db.GetRating(testDeal);
            Assert.That(rating, Is.EqualTo(1));
        }
    }
}
