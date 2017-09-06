using System.Web.Mvc;
using System.Web.Routing;

namespace BooksEditor
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "Page1",
                defaults: new { controller = "Book", action = "Index" }
            );

            routes.MapRoute(
                name: null,
                url: "Page{page}/Sort{order}/Type{orderType}",
                defaults: new { controller = "Book", action = "BooksList" },
                constraints: new { page = @"\d+" }
            );

            routes.MapRoute(
                name: null,
                url: "CreateBook",
                defaults: new { controller = "Book", action = "CreateBook", book_id = 0 }
            );

            routes.MapRoute(
                name: null,
                url: "EditBook/{book_id}",
                defaults: new { controller = "Book", action = "EditBook" },
                constraints: new { book_id = @"\d+" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Book", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
