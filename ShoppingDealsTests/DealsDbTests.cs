using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            db = new DealsDb("test");
            await db.Reinitialize();
        }

        private async Task AddTestDeal()
        {
            Deal testDeal = new Deal(
                username: "Jared",
                productName: "Nintendo 3DS",
                price: 50.00m,
                storeName: "Amazon",
                zipCode: "1234",
                expirationDate: new DateTime(2016, 5, 18, 6, 32, 0)
            );
            await db.AddDeal(testDeal);
        }

        private async Task AddOtherTestDeal()
        {
            Deal testDeal = new Deal(
                username: "Mr. Lemon",
                productName: "a pear of socks",
                price: 0.25m,
                storeName: "Amazon",
                zipCode: "9876",
                expirationDate: new DateTime(2017, 2, 8, 18, 20, 1)
            );
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
    }
}
