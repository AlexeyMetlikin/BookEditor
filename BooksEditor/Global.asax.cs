using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using BooksEditor.Models.Infrastructure;
using BooksEditor.App_Start;

namespace BooksEditor
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new NinjectContollerFactory());
        }
    }
}
