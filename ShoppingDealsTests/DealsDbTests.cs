using System;
using System.Collections.Generic;
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
            db = new DealsDb();
            db.Reinitialize();
            await AddTestDeal();
        }

        private async Task AddTestDeal()
        {
            Deal testDeal = new Deal(
                username: "Jared",
                productName: "Nintendo 3DS",
                price: 50.00m,
                storeName: "Amazon",
                zipCode: 1234,
                expirationDate: new DateTime(2016, 5, 18, 6, 32, 0)
            );
            await db.AddDeal(testDeal);
        }

        [Test]
        public async Task TestGet()
        {
            IEnumerable<Deal> results = await db.GetDeals();
        }

        [Test]
        public async Task TestDuplicate()
        {
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
    }
}
