using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Ums.App;
using Ums.Services.OAuth;

namespace Ums.Website.Security
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
            var app = UnityConfig.Resolve<IOAuthApplicationService>().GetByToken(token);
            HttpContext.Current.Items["CurrentApplication"] = app;
            return app != null;
        }
    }
}
