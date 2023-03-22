using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Helper;
using Ums.App.Security;
using Ums.Core.Domain.Connect;
using Ums.Models.Common;
using Ums.Models.Connect;
using Ums.Services.Connect;
using Ums.Services.System;

namespace Ums.Website.Controllers
{
    public class ConnectController : BaseController
    {
        private readonly INoticeService _noticeService;
        private readonly IFeedbackService _feedbackService;
        private readonly IRoleService _roleService;
        public ConnectController(INoticeService noticeService, IFeedbackService feedbackService, IRoleService roleService)
        {
            _noticeService = noticeService;
            _feedbackService = feedbackService;
            _roleService = roleService;
        }
        
        #region Notice
        [Function("NOTICE")]
        public ActionResult Notice()
        {
            return View();
        }

        [Function("NOTICE")]
        public object GetNotice(TableModel model)
        {
            var lst = _noticeService.Gets().OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    Department = i.Department.Name,
                    Staff = i.Staff.Name,
                    i.Public
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("NOTICE")]
        public ActionResult NoticeEdit(int id = 0)
        {
            ViewBag.Roles = _roleService.Gets().OrderBy(i => i.Name).Select(i => new IdNameModel { Id = i.Id, Name = i.Name }).ToList();
            var m = new NoticeModel();
            if (id == 0) return View(m);
            var d = _noticeService.Get(id);
            d.CopyTo(m);
            m.RoleIds = d.RoleIds;
            return View(m);
        }

        [Function("NOTICE")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult NoticeEdit(NoticeModel model, FormCollection form)
        {
            var d = new Notice();
            model.CopyTo(d);
            d.StaffId = WorkContext.Staff.Id;
            d.DepartmentId = WorkContext.Department.Id;
            d.RoleData = form.GetValues("role[]")?.Select(int.Parse).ToJson();
            _noticeService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("NOTICE")]
        public ActionResult NoticeDelete(int id)
        {
            _noticeService.Delete(id);
            return RedirectToAction("Notice");
        }

        public object GetLastNotify()
        {
            var roles = WorkContext.UserInfo.UsersRoles.Where(i => !i.IsDeleted).Select(i => i.RoleId).ToArray();
            var ns = _noticeService.GetNotices().Where(i => !string.IsNullOrEmpty(i.RoleData)).OrderByDescending(i => i.Id).ToList();
            var n = ns.FirstOrDefault(i => roles.Any(j => i.RoleIds.Contains(j)));
            return n != null ? new { n.Name, n.Content }.ToJson() : "";
        }
        #endregion

        #region Feedback
        [Function("FEEDBACK")]
        public ActionResult Feedback()
        {
            return View();
        }

        [Function("FEEDBACK")]
        public object GetFeedback(TableModel model)
        {
            var lst = _feedbackService.Gets().OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Title,
                    i.Content,
                    Department = i.Department.Name,
                    Staff = i.Staff.Name,
                    i.Response,
                    ResponseOn = string.IsNullOrEmpty(i.Response) ? "" : i.Responsed.ToAppDateTime(),
                    Updated = i.Updated.ToAppDateTime(),
                    Responser = i.Responser?.Name
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("FEEDBACK")]
        public ActionResult FeedbackReply(int id = 0)
        {
            var m = new FeedbackModel();
            if (id == 0) return View(m);
            var d = _feedbackService.Get(id);
            d.CopyTo(m);
            return View(m);
        }

        [Function("FEEDBACK")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FeedbackReply(FeedbackModel model)
        {
            _feedbackService.Response(model.Id, model.Response);
            return IframeScript;
        }

        [Function("FEEDBACK")]
        public ActionResult FeedbackDelete(int id)
        {
            _feedbackService.Delete(id);
            return RedirectToAction("Feedback");
        }
        #endregion
    }
}