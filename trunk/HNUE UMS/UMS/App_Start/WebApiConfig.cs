using System.Web.Http;

namespace Ums.Website
{
    public static class WebApiConfig
    {
        public static void RegisterRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("Api", "api_v2/{controller}/{action}", new { action = "Index" });
        }
    }
}