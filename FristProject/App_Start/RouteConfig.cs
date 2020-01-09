using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FristProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "ZYCRoute",
                url: "2019/1025",
                defaults: new { controller = "SeeMore", action = "ZYCIndex" }
            );

            routes.MapRoute(
                name: "GHCGRoute",
                url: "2019/test",
                defaults: new { controller = "SeeMore", action = "Login" }
            );
            routes.MapRoute(
               name: "GHCGRoute2",
               url: "2019/1105",
               defaults: new { controller = "SeeMore", action = "GHCGIndex" }
           );
            routes.MapRoute(
               name: "HXC2019SDZL",
               url: "2019/1212C",
               defaults: new { controller = "HXC", action = "HXC2019SHZL" }
           );

            routes.MapRoute(
             name: "HXC2019SDZLS",
             url: "2019/1212CC",
             defaults: new { controller = "HXC", action = "HXC2019SHZL" }
         ); 
            routes.MapRoute(
              name: "JYC2020XNYS",
              url: "2020/0109",
              defaults: new { controller = "ZGTJJYC", action = "Index" }
          );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


        }
    }
}
