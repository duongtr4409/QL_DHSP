using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Security;
using Ums.Core.Domain.Organize;
using Ums.Core.Entities.Shared;
using Ums.Models.Common;
using Ums.Models.Organize;
using Ums.Services.Data;
using Ums.Services.Organize;

namespace Ums.Website.Controllers
{
    public class OrganizeController : BaseController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IPositionService _positionService;
        private readonly IPositionGroupService _positionGroupService;
        public OrganizeController(IDepartmentService departmentService, IPositionService positionService, IPositionGroupService positionGroupService)
        {
            _departmentService = departmentService;
            _positionService = positionService;
            _positionGroupService = positionGroupService;
        }

        #region DEPARTMENT
        [Function("ORG_DEPARTMENT")]
        public ActionResult Department()
        {
            return View();
        }

        [Function("ORG_DEPARTMENT")]
        public object GetDepartment(TableModel model)
        {
            var lst = _departmentService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize)
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.SyncCode,
                    i.Code,
                    i.Email,
                    i.Tel
                }).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("ORG_DEPARTMENT")]
        public ActionResult DepartmentEdit(int id = 0)
        {
            ViewBag.Groups = _positionGroupService.GetTree();
            var m = new DepartmentModel();
            if (id == 0) return View(m);
            var d = _departmentService.Get(id);
            d.CopyTo(m);
            ViewBag.PositionId = d.MaxPositions.FirstOrDefault()?.Key;
            ViewBag.Max = d.MaxPositions.FirstOrDefault()?.Value;
            ViewBag.GroupId = _positionService.Get(d.MaxPositions.FirstOrDefault()?.Key ?? 0)?.GroupId;
            return View(m);
        }

        [Function("ORG_DEPARTMENT")]
        [HttpPost]
        public ActionResult DepartmentEdit(DepartmentModel model, FormCollection form)
        {
            if (!ModelState.IsValid) return View(model);
            var d = _departmentService.Get(model.Id) ?? new Department();
            model.CopyTo(d);
            d.PositionMax = new[]
            {
                new IdId
                {
                    Key = form.GetValue("PositionId")?.AttemptedValue?.ToInt() ?? 0,
                    Value = form.GetValue("Max")?.AttemptedValue?.ToInt() ?? 0,
                }
            }.ToJson();
            _departmentService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("ORG_DEPARTMENT")]
        public ActionResult DepartmentDelete(int id)
        {
            var d = _departmentService.Get(id);
            _departmentService.Delete(d);
            return RedirectToAction("Department");
        }

        [Function("ORG_DEPARTMENT")]
        public JsonResult GetPositions(int groupId)
        {
            return Json(_positionService.Gets(groupId).Select(i => new { i.Id, i.Name }).ToArray(), JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
