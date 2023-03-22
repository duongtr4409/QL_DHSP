using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Ums.Services.System;

namespace Ums.App.Security
{
    public class ApiAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext httpContext)
        {
            if (!httpContext.Request.Headers.Contains("token"))
            {
                return false;
            }
            var token = httpContext.Request.Headers.GetValues("token").First();
            var app = UnityConfig.Resolve<IApplicationService>().GetByToken(token);
            HttpContext.Current.Items ["CurrentApplication"] = app;
            return app != null;
        }
    }
}
