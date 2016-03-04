using System.Threading.Tasks;
using NUnit.Framework;
using ShoppingDeals.Controllers;

namespace ShoppingDealsTests
{
    [TestFixture]
    partial class DealsDbTests
    {
        private DealsDb db;

        [SetUp]
        public async Task InitDb()
        {
            db = new DealsDb("shoppingdeals-test");
            await db.Reinitialize();
        }
    }
}
