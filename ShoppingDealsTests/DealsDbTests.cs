﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShoppingDeals.Controllers;
using ShoppingDeals.Models;

namespace ShoppingDealsTests
{
    [TestClass]
    public class DealsDbTests
    {
        private DealsDb db;

        [TestInitialize]
        public void InitDb()
        {
            db = new DealsDb();
            db.Reinitialize();
            Task res = AddTestDeal();
            res.Wait();
        }

        private async Task AddTestDeal()
        {
            Deal testDeal = new Deal(
                username: "Jared",
                productName: "Nintendo 3DS",
                price: 50.00m,
                storeName: "Amazon",
                zipCode: 1234,
                expirationDate: DateTime.Now.AddYears(1)
            );
            await db.AddDeal(testDeal);
        }

        [TestMethod]
        public async Task TestGet()
        {
            IEnumerable<Deal> results = await db.GetDeals();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task TestDuplicate()
        {
            await AddTestDeal(); //again
            //TODO: make this fail
        }
    }
}
