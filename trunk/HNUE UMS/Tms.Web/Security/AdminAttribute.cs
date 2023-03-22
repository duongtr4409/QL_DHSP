using System.Web;
using System.Web.Mvc;
using Ums.App.Base;

namespace Ums.App.Security
{
    public class AdminAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            return isAuthorized && WorkContext.UserInfo.IsAdmin;
        }
    }
}
