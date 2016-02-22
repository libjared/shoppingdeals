using System;
using MongoDB.Bson;

namespace ShoppingDeals.Models
{
    public class Deal
    {
        public Deal(string username, string productName, decimal price, string storeName, string zipCode, DateTime expirationDate)
        {
            Username = username;
            ProductName = productName;
            Price = price;
            StoreName = storeName;
            ZipCode = zipCode;
            ExpirationDate = expirationDate;
            Likes = 0;
            Dislikes = 0;
        }

        public string ProductName { get; private set; }
        public string StoreName { get; private set; }
        public decimal Price { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public string Username { get; private set; }
        public string ZipCode { get; private set; }
        public int Likes { get; private set; }
        public int Dislikes { get; private set; }

        public ObjectId Id { get; private set; }

        public void Like()
        {
            Likes++;
        }

        public void Dislike()
        {
            Dislikes++;
        }
    }
}