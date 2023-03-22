using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.Core.Domain.File;
using Ums.Models.Common;
using Ums.Models.File;
using Ums.Services.Connect;
using Ums.Services.Users;

namespace Ums.Website.Controllers
{
    public class FileController : BaseController
    {
        private readonly INoticeService _noticeService;
        private readonly IFileContentService _fileContentService;
        private readonly IFileAccessService _fileAccessService;
        private readonly IUserService _userService;
        public FileController(INoticeService noticeService, IFileContentService fileContentService, IFileAccessService fileAccessService, IUserService userService)
        {
            _noticeService = noticeService;
            _fileContentService = fileContentService;
            _fileAccessService = fileAccessService;
            _userService = userService;
        }

        public ActionResult Content(int parentId = 0)
        {
            ViewBag.ParentId = parentId;
            return View();
        }

        public object GetContent(TableModel model, int parentId = 0)
        {
            var lst = _fileContentService.Gets().Where(i => i.ParentId == parentId && (i.CreatorId == WorkContext.UserInfo.Id || i.Accesses.Any(j => !j.IsDeleted && j.UserId == WorkContext.UserInfo.Id))).OrderByDescending(i => string.IsNullOrEmpty(i.FileUrl)).ThenBy(i => i.Name);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult FolderEdit(int id = 0, int parentId = 0)
        {
            var m = new FileContentModel { ParentId = parentId };
            if (id == 0) return View(m);
            var d = _fileContentService.Get(id);
            d.CopyTo(m);
            return View(m);
        }

        public ActionResult ContentEdit(int id = 0, int parentId = 0)
        {
            var m = new FileContentModel { ParentId = parentId };
            if (id == 0) return View(m);
            var d = _fileContentService.Get(id);
            d.CopyTo(m);
            return View(m);
        }

        [HttpPost]
        public ActionResult ContentEdit(FileContentModel model, FormCollection form)
        {
            var d = new FileContent();
            model.CopyTo(d);
            d.CreatorId = WorkContext.UserInfo.Id;
            var files = form.GetValues("file[]") ?? new string[0];
            var names = form.GetValues("name[]") ?? new string[0];
            if (files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    d.Name = names[i];
                    d.FileUrl = files[i];
                    _fileContentService.Insert(d);
                }
            }
            else
            {
                _fileContentService.InsertOrUpdate(d);
            }
            return IframeScript;
        }

        public ActionResult ContentDelete(int id)
        {
            _fileContentService.Delete(id);
            return RedirectToAction("Content");
        }

        public ActionResult ContentShare(int id)
        {
            var m = new ContentShareModel { FileId = id };
            m.UserIds = _fileAccessService.Gets().Where(i => i.FileId == id).Select(i => i.UserId).ToArray();
            if (m.UserIds.Length > 0)
            {
                ViewBag.Users = _userService.Gets().Where(i => m.UserIds.Contains(i.Id)).Select(i => new { id = i.Id, value = i.Name }).ToArray().ToJson();
            }
            return View(m);
        }

        [HttpPost]
        public ActionResult ContentShare(ContentShareModel model)
        {
            foreach (var j in model.UserIds)
            {
                var a = _fileAccessService.Gets().FirstOrDefault(i => i.FileId == model.FileId && i.UserId == j) ?? new FileAccess { FileId = model.FileId, UserId = j };
                _fileAccessService.InsertOrUpdate(a);
            }
            foreach (var i in _fileAccessService.Gets().Where(i => !model.UserIds.Contains(i.UserId)).ToList())
            {
                _fileAccessService.Delete(i);
            }
            return IframeScript;
        }


        public JsonResult GetRunner(string term = "")
        {
            var data = _userService.Gets().Where(i => i.Name.Contains(term)).ToList();
            return Json(data.Select(i => new
            {
                id = i.Id,
                value = i.Name
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}
