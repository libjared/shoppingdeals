using System.Web.Http;

namespace ShoppingDeals
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            InitControllerSync();
        }

        private static void InitControllerSync()
        {
            Controllers.DealsController.Initialize();
            Controllers.UserController.Initialize();
        }
    }
}