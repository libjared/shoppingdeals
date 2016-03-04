using System;
using MongoDB.Bson;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

namespace ShoppingDeals.Models
{
    public class Rating
    {
        public ObjectId AboutDeal { get; set; }
        public ObjectId AboutUser { get; set; }
        public bool IsPositive { get; set; }
        public DateTime ExpirationDate { get; set; }

        public ObjectId Id { get; set; }

        public Rating(Deal aboutDeal, User aboutUser, bool isPositive)
        {
            AboutDeal = aboutDeal.Id;
            AboutUser = aboutUser.Id;
            IsPositive = isPositive;
            ExpirationDate = aboutDeal.ExpirationDate;
        }
    }
}