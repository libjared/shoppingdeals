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
            var task = Controllers.DealsController.Initialize();
            task.ConfigureAwait(false);
            task.Wait();
        }
    }
}