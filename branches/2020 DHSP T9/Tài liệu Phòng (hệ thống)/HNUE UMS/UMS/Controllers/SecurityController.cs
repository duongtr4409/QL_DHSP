using System;
using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.Models.Common;
using Ums.Services.System;
using Ums.Services.Users;

namespace Ums.Website.Controllers
{
    public class SecurityController : BaseController
    {
        private readonly ISessionService _sessionService;
        private readonly ISettingService _settingService;
        public SecurityController(ISessionService userService, ISettingService settingService)
        {
            _sessionService = userService;
            _settingService = settingService;
        }

        public ActionResult Online(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        public object GetOnline(TableModel model)
        {
            var onlineTime = _settingService.GetValue("SECURITY_ONLINE_TIME").ToInt();
            var time = DateTime.Now.AddMinutes(onlineTime);
            var lst = _sessionService.Gets().Where(i => i.LastAccess >= time);
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public new ActionResult Session(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        public ActionResult Logon(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        public object GetSessions(TableModel model)
        {
            var lst = _sessionService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public void SessionDelete(int id)
        {
            var u = _sessionService.Get(id);
            _sessionService.Delete(u);
        }
    }
}