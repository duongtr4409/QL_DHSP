using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TEMIS.CMS.Common;

namespace TEMIS.CMS
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.Add("chitiettintuc", new SeoFriendlyRoute("tin-tuc/{id}",
                                    new RouteValueDictionary(new { controller = "Home", action = "TinTuc" }),
                                    new MvcRouteHandler()));

            routes.Add("chitietthongbao", new SeoFriendlyRoute("thong-bao/{id}",
                                   new RouteValueDictionary(new { controller = "Home", action = "ThongBao" }),
                                   new MvcRouteHandler()));

            routes.Add("Login", new SeoFriendlyRoute("dang-nhap", new RouteValueDictionary(new { controller = "Account", action = "Login" }), new MvcRouteHandler()));
            routes.Add("Register", new SeoFriendlyRoute("dang-ky", new RouteValueDictionary(new { controller = "Account", action = "Register" }), new MvcRouteHandler()));
            routes.Add("logoff", new SeoFriendlyRoute("dang-xuat", new RouteValueDictionary(new { controller = "Account", action = "logoff" }), new MvcRouteHandler()));
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
