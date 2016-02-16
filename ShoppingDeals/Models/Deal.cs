using System;

namespace ShoppingDeals.Models
{
    public class Deal
    {
        //username, prod name, price, store name, zip code, expiration date, like count, dislike count
        public string Username { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string StoreName { get; set; }
        public int ZipCode { get; set; }
        public DateTime ExpirationDate;
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}