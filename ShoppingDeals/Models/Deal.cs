using System;
using MongoDB.Bson;

namespace ShoppingDeals.Models
{
    public class Deal
    {
        public string ProductName { get; set; }
        public string StoreName { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Username { get; set; }
        public string ZipCode { get; set; }
        public int Likes { get; private set; }
        public int Dislikes { get; private set; }

        public ObjectId Id { get; private set; }
    }
}