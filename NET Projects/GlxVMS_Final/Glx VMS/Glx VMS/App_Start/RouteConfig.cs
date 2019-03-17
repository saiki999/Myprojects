using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Glx_VMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "HomeRoute",
                "Home/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, sponsor = string.Empty }
            );
            routes.MapRoute(
                "EmployeeRoute",
                "Employee/{action}/{id}", // URL with parameters
                new { controller = "Employee", action = "Index", id = UrlParameter.Optional, sponsor = string.Empty }
            );
            routes.MapRoute(
                "VisitorReportRoute",
                "Visitor/{action}/{id}", // URL with parameters
                new { controller = "Visitor", action = "Index", id = UrlParameter.Optional, sponsor = string.Empty }
            );
            routes.MapRoute(
                name: "locationRoute",
                url: "{location}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                "NonlocationRoute",
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, sponsor = string.Empty }
           );
        }
    }
}
