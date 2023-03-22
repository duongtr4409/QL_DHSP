using Hnue.Helper;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Ums.Core.Domain.Users;

namespace Ums.App.Base
{
    [Authorize]
    public class BaseController : Controller
    {
        public User UserInfo => WorkContext.UserInfo;

        public ContentResult IframeScript
        {
            get
            {
                var addNew = Request.Form.Get("act_continue")?.Split(',')[0].ToBool() ?? false;
                if (!addNew)
                {
                    return Content("<script>window.parent.location.href=window.parent.location.href;</script>");
                }
                return Content($"<script>window.location.href='{Regex.Replace(Request.UrlReferrer.PathAndQuery, @"id=([\d])+", "id=0")}';</script>");
            }
        }
    }
}