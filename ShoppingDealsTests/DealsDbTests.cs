using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ShoppingDeals.Controllers;
using ShoppingDeals.Models;

namespace ShoppingDealsTests
{
    [TestFixture]
    public class DealsDbTests
    {
        private DealsDb db;

        [SetUp]
        public async Task InitDb()
        {
            db = new DealsDb("shoppingdeals-test");
            await db.Reinitialize();
        }

        private async Task AddTestDeal()
        {
            var testDeal = new Deal
            {
                Username = "Jared",
                ProductName = "Nintendo 3DS",
                Price = 50.00m,
                StoreName = "Amazon",
                ZipCode = "1234",
                ExpirationDate = new DateTime(2016, 5, 18, 6, 32, 0)
            };
            await db.AddDeal(testDeal);
        }

        private async Task AddOtherTestDeal()
        {
            var testDeal = new Deal
            {
                Username = "Mr. Lemon",
                ProductName = "a pear of socks",
                Price = 0.25m,
                StoreName = "Amazon",
                ZipCode = "9876",
                ExpirationDate = new DateTime(2017, 2, 8, 18, 20, 1)
            };
            await db.AddDeal(testDeal);
        }

        [Test]
        public async Task TestGet()
        {
            await AddTestDeal();
            var results = await db.GetDeals();
            Assert.That(results.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task TestDuplicate()
        {
            await AddTestDeal();
            try
            {
                await AddTestDeal();
            }
            catch (ArgumentException)
            {
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.ToString());
            }
            Assert.Fail("No exception thrown");
        }

        [Test]
        [TestCase("a pear of socks", "Amazon", "9876", 1)]
        [TestCase("Nintendo 3DS", "Amazon", "1234", 1)]
        [TestCase(null, "Amazon", "1234", 1)]
        [TestCase(null, "Amazon", null, 2)]
        public async Task TestGetSearch(string prod, string store, string zip, int expectedCount)
        {
            await AddTestDeal();
            await AddOtherTestDeal();

            var results = await db.GetDeals(prod, store, zip);
            var deals = results.ToList();
            Assert.That(deals.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        public void TestDealFromJson()
        {
            /*
            public string ProductName { get; set; }
            public string StoreName { get; set; }
            public decimal Price { get; set; }
            public DateTime ExpirationDate { get; set; }
            public string ZipCode { get; set; }
            */
            //convert from json to PostedDeal to Deal
            var json = "{" +
                       "\"ProductName\": \"Bread\"," +
                       "\"StoreName\": \"Bread Store\"," +
                       "\"Price\": \"10.00\"," +
                       "\"ExpirationDate\": \"2016-02-29T01:02:03Z\"," + //1am utc
                       "\"ZipCode\": \"49503\"" +
                       "}";
            var token = JToken.Parse(json);
            var postedDeal = token.ToObject<PostedDeal>();
            var deal = postedDeal.ToDeal();

            Assert.That(deal.ProductName, Is.EqualTo("Bread"));
            Assert.That(deal.Price, Is.EqualTo(10.00m));
            Assert.That(deal.ZipCode, Is.EqualTo("49503"));
            Assert.That(deal.ExpirationDate, Is.EqualTo(new DateTime(2016, 2, 29, 1, 2, 3, DateTimeKind.Utc)));
        }
    }
}
