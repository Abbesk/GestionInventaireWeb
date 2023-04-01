using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Inventaire_BackEnd
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
           routes.MapRoute("DefaultApiWithId", "Api/{controller}/{id}", new { id = UrlParameter.Optional }, new { id = @"\d+" });
            routes.MapRoute("DefaultApiWithAction", "Api/{controller}/{action}");
           
        }
    }
}
