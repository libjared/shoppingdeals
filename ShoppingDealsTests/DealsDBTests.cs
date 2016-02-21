using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers.Tests
{
    [TestClass]
    public class DealsDBTests
    {
        private static DealsDB db;

        [TestInitialize]
        public void InitDB()
        {
            db = new DealsDB();
            db.Reinitialize();
        }

        [TestMethod]
        public async Task TestGet()
        {
            IEnumerable<Deal> results = await db.GetDeals();
        }
    }
}
