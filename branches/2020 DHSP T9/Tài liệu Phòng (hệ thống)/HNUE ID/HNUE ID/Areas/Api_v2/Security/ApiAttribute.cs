using System.Web;
using System.Web.Mvc;
using Ums.App;
using Ums.Services.System;

namespace Ums.Website.Areas.Api_v2.Security
{
    public class ApiAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (UnityConfig.Resolve<IApplicationService>().Validate(httpContext.Request.Headers.Get("token")))
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
