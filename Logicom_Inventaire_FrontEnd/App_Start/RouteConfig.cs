using System.Web.Mvc;
using System.Web.Routing;

namespace Logicom_Inventaire_FrontEnd
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Authentifier", id = UrlParameter.Optional }
            );
        }
    }
}
