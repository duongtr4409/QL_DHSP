using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TEMIS.Model;
namespace TEMIS.CMS.Common
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var session = HttpContext.Current.Session;

            if (session[PublicConstant.ROLE_INFO] == null)
            {
                filterContext.Result = new RedirectResult(string.Format("/dang-nhap/"));
            }
            else
            {
                var role = (UserRoles)session[PublicConstant.ROLE_INFO];
                if (!Roles.Contains(role.Role))
                {
                    filterContext.Result = new RedirectResult(string.Format("/dang-nhap/"));
                }
            }
        }
    }
}