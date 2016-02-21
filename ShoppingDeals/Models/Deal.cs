using System;

namespace ShoppingDeals.Models
{
    public class Deal
    {
        public Deal(string username, string productName, decimal price, string storeName, int zipCode, DateTime expirationDate)
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

        public string ProductName { get; }
        public string StoreName { get; }
        public decimal Price { get; }
        public DateTime ExpirationDate { get; }
        public string Username { get; }
        public int ZipCode { get; }
        public int Likes { get; private set; }
        public int Dislikes { get; private set; }

        public int Id => GetHashCode();

        public void Like()
        {
            Likes++;
        }

        public void Dislike()
        {
            Dislikes++;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = 0;
                hashCode = (hashCode * 397) ^ (ProductName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ (StoreName?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Price.GetHashCode();
                hashCode = (hashCode * 397) ^ ExpirationDate.GetHashCode();
                return hashCode;
            }
        }
    }
}