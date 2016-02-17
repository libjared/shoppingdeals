using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingDeals.Models;

namespace ShoppingDeals.Controllers
{
    public interface IDealsDB
    {
        void AddDealAsync(Deal deal);

        Task<IEnumerable<Deal>> GetDealsAsync();
    }
}