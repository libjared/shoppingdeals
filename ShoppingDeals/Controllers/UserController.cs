using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ShoppingDeals.Models;

// ReSharper disable UnusedMember.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace ShoppingDeals.Controllers
{
    [RoutePrefix("api/v1/user")]
    public class UserController : ApiController
    {
        private static DealsDb db;

        public static void Initialize()
        {
            StaticDealsDb.Initialize("shoppingdeals");
            db = StaticDealsDb.Db;
        }

        [Route("register")]
        [HttpPost]
        public async Task<HttpResponseMessage> Register([FromBody]RegisterLoginUser user)
        {
            try
            {
                await db.RegisterUser(user.Name, user.Password);
            }
            catch (AlreadyExistsException)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("login")]
        [HttpPost]
        public async Task<HttpResponseMessage> Login([FromBody]RegisterLoginUser user)
        {
            Guid apikey;
            try
            {
                apikey = await db.LoginUser(user.Name, user.Password);
            }
            catch (CredentialsException)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
            return Request.CreateResponse(HttpStatusCode.OK, apikey);
        }
    }
}