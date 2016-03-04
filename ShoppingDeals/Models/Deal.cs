using System;
using MongoDB.Bson;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

namespace ShoppingDeals.Models
{
    public class Deal
    {
        public string ProductName { get; set; }
        public string StoreName { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public ObjectId UserPostedBy { get; set; }
        public string ZipCode { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        public ObjectId Id { get; set; }
    }

    public class PostedDeal
    {
        public string ProductName { get; set; }
        public string StoreName { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ZipCode { get; set; }

        public Deal ToDeal(User postedBy)
        {
            return new Deal
            {
                ProductName = ProductName,
                StoreName = StoreName,
                Price = Price,
                ExpirationDate = ExpirationDate,
                ZipCode = ZipCode,
                UserPostedBy = postedBy.Id
            };
        }
    }
}