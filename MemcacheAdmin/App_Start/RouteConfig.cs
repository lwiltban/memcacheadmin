using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MemcacheAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

            );

            routes.MapRoute(
                name: "Slabs",
                url: "{controller}/{action}/{serverId}/{slabId}",
                defaults: new { controller = "Home", action = "Slab", serverId = UrlParameter.Optional, slabId = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Deletes",
                url: "{controller}/{action}/{serverId}/{slabId}/{key}",
                defaults: new { controller = "Home", action = "AjaxDelete", serverId = UrlParameter.Optional, slabId = UrlParameter.Optional, key = UrlParameter.Optional }
                );

        }
    }
}