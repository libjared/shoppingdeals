using System;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using ShoppingDeals.Models;

namespace ShoppingDealsTests
{
    public partial class DealsDbTests
    {
        private async Task AddTestDeal()
        {
            var testDeal = new Deal
            {
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
                ProductName = "a pear of socks",
                Price = 0.25m,
                StoreName = "Amazon",
                ZipCode = "9876",
                ExpirationDate = new DateTime(2017, 2, 8, 18, 20, 1)
            };
            await db.AddDeal(testDeal);
        }

        [Test]
        public async Task TestDealsGet()
        {
            await AddTestDeal();
            var results = await db.GetDeals();
            Assert.That(results.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task TestDealsDuplicate()
        {
            await AddTestDeal();
            try
            {
                await AddTestDeal();
            }
            catch (AlreadyExistsException)
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
        public async Task TestDealsGetSearch(string prod, string store, string zip, int expectedCount)
        {
            await AddTestDeal();
            await AddOtherTestDeal();

            var results = await db.GetDeals(prod, store, zip);
            var deals = results.ToList();
            Assert.That(deals.Count, Is.EqualTo(expectedCount));
        }

        [Test]
        [Timeout(1000*60*2)] //better be cleaned up in 2 minutes
        public async Task TestDealExpire()
        {
            var testDeal = new Deal
            {
                ProductName = "almost-expired milk",
                Price = 0.05m,
                StoreName = "Walmart",
                ZipCode = "9876",
                ExpirationDate = DateTime.Now.AddSeconds(5)
            };
            await db.AddDeal(testDeal);
            await db.RateDeal(testDeal, new User("user", "pass"), true);

            //see if it's still there
            var found = db.GetSpecificDeal(testDeal.StoreName, testDeal.ProductName, testDeal.ExpirationDate, testDeal.Price);
            Assert.That(found, Is.Not.Null);

            var startedChecking = DateTime.Now;

            Deal foundTwo;
            do
            {
                //wait a while
                await Task.Delay(new TimeSpan(0, 0, 1));

                //it should be gone
                foundTwo =
                    await
                        db.GetSpecificDeal(testDeal.StoreName, testDeal.ProductName, testDeal.ExpirationDate,
                            testDeal.Price);
            } while (foundTwo != null);

            var timeTaken = DateTime.Now - startedChecking;
            Console.WriteLine($"Should have taken 5 seconds but {timeTaken.TotalMilliseconds} ms is fine too");
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
            var deal = postedDeal.ToDeal(new User("user", "pass"));

            Assert.That(deal.ProductName, Is.EqualTo("Bread"));
            Assert.That(deal.Price, Is.EqualTo(10.00m));
            Assert.That(deal.ZipCode, Is.EqualTo("49503"));
            Assert.That(deal.ExpirationDate, Is.EqualTo(new DateTime(2016, 2, 29, 1, 2, 3, DateTimeKind.Utc)));
        }
    }
}
