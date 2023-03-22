using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using Hnue.Helper;
using Ums.Core.Domain.System;

namespace Ums.App.Base
{
    [Authorize]
    public class BaseController : Controller
    {
        public StaffUser UserInfo => WorkContext.UserInfo;
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

        public ContentResult SaveMore(params string[] prms)
        {
            var addNew = Request.Form.Get("act_continue")?.Split(',')[0].ToBool() ?? false;
            if (!addNew)
            {
                return Content("<script>window.parent.location.href=window.parent.location.href;</script>");
            }
            var url = Regex.Replace(Request.UrlReferrer.PathAndQuery, @"id=([\d])+", "id=0");
            url = url.IndexOf('?') >= 0 ? url + "&" + prms.JoinArray("&") : url + "?" + prms.JoinArray("&");
            return Content($"<script>window.location.href='{url}';</script>");
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            var f = WorkContext.Functions.FirstOrDefault(i => i.Url == requestContext.HttpContext.Request.Url?.LocalPath || i.Url == requestContext.HttpContext.Request.Url?.LocalPath + "/Index");
            if (f != null)
            {
                ViewBag.Title = f.Name;
                ViewBag.PageIcon = f.Icon;
            }
            return base.BeginExecute(requestContext, callback, state);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}