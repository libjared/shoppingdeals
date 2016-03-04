using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using ShoppingDeals.Models;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/deals")]
    public class DealsController : ApiController
    {
        private static DealsDb db;

        public static void Initialize()
        {
            StaticDealsDb.Initialize("shoppingdeals");
            db = StaticDealsDb.Db;
        }

        [Route("")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetDeals(string prod = null, string store = null, string zip = null)
        {
            var authUser = GetThisAuthenticatedUser();
            if (authUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var data = await db.GetDeals(prod, store, zip);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

        [Route("")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostDeal(PostedDeal postedDeal)
        {
            var authUser = GetThisAuthenticatedUser();
            if (authUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            await db.AddDeal(postedDeal.ToDeal(authUser));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [Route("like")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostLike(RatingRequest ratingRequest)
        {
            return await Rate(ratingRequest, true);
        }

        [Route("dislike")]
        [HttpPost]
        public async Task<HttpResponseMessage> PostDislike(RatingRequest ratingRequest)
        {
            return await Rate(ratingRequest, false);
        }

        private async Task<HttpResponseMessage> Rate(RatingRequest ratingRequest, bool isPositive)
        {
            var authUser = GetThisAuthenticatedUser();
            if (authUser == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var theDeal =
                await
                    db.GetSpecificDeal(ratingRequest.StoreName, ratingRequest.ProductName, ratingRequest.ExpirationDate,
                        ratingRequest.Price);

            await db.RateDeal(theDeal, authUser, isPositive);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private User GetThisAuthenticatedUser()
        {
            //check if apikey is in the headers
            IEnumerable<string> headerValues;
            if (!Request.Headers.TryGetValues("X-Deals-ApiKey", out headerValues))
            {
                return null;
            }

            //check if apikey header has a value
            var apikey = headerValues.FirstOrDefault();
            if (apikey == null)
            {
                return null;
            }

            //check if apikey is a guid
            Guid apikeyGuid;
            if (!Guid.TryParse(apikey, out apikeyGuid))
            {
                return null;
            }

            //check if the apikey is real
            var thisUser = db.GetUserFromApiKey(apikeyGuid);

            return thisUser;
        }
    }
}