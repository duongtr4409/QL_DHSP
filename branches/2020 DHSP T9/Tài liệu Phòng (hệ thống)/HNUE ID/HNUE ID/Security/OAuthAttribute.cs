using System.Web;
using System.Web.Mvc;
using Ums.App;
using Ums.Services.OAuth;

namespace Ums.Website.Security
{
    public class OAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (UnityConfig.Resolve<IOAuthApplicationService>().Validate(httpContext.Request.Headers.Get("token")))
            {
                return true;
            }
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Write("Access denied");
            httpContext.Response.End();
            return false;
        }
    }
}
