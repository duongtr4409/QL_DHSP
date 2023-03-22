using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ums.App.Base;

namespace Ums.App.Security
{
    public class FunctionAttribute : AuthorizeAttribute
    {
        private readonly string[] _accessKeys;

        public FunctionAttribute(params string[] keys)
        {
            _accessKeys = keys;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            return isAuthorized && WorkContext.Functions.Any(i => _accessKeys.Any(j => j == i.Key));
        }
    }
}
