using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Helper;
using Ums.App.Security;
using Ums.Core.Domain.Report;
using Ums.Core.Types;
using Ums.Models.Common;
using Ums.Models.Statistic;
using Ums.Services.Calculate;
using Ums.Services.Data;
using Ums.Services.Organize;
using Ums.Services.Report;
using Ums.Services.Statistic;

namespace Ums.Website.Controllers
{
    public class StatisticController : BaseController
    {
        private readonly IYearService _yearService;
        private readonly IDepartmentService _departmentService;
        private readonly IGradeService _gradeService;
        private readonly IReportService _reportService;
        private readonly ITypeService _typeService;
        private readonly ISemesterService _semesterService;
        private readonly IStatisticService _statisticService;
        public StatisticController(IYearService yearService, IDepartmentService departmentService, IGradeService gradeService, IReportService reportService, ITypeService typeService, ISemesterService semesterService, IStatisticService statisticService)
        {
            _yearService = yearService;
            _departmentService = departmentService;
            _gradeService = gradeService;
            _reportService = reportService;
            _typeService = typeService;
            _semesterService = semesterService;
            _statisticService = statisticService;
        }

        #region HOME
        [Function("STATISTIC_HOME")]
        public ActionResult Embed()
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.Departments = _departmentService.Gets(new[] { DepartmentTypes.Faculty });
            ViewBag.Grades = _gradeService.Gets();
            return View();
        }

        [Function("STATISTIC_HOME")]
        public ActionResult Home()
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.Departments = _departmentService.Gets(new[] { DepartmentTypes.Faculty });
            ViewBag.Grades = _gradeService.Gets();
            return View();
        }

        [Function("STATISTIC_HOME")]
        public object GetStatistic(int yearId, int departmentId, int gradeId)
        {
            var data = _statisticService.StaveStatistic(yearId, departmentId, gradeId);
            data.CalculateDone();
            return data.ToJson();
        }
        #endregion

        #region REPORT
        [Function("STATISTIC_REPORT")]
        public ActionResult Report(int yearId = 0, int typeId = 0)
        {
            var years = _yearService.Gets();
            ViewBag.Years = years;
            ViewBag.YearId = yearId > 0 ? yearId : years.FirstOrDefault()?.Id;
            ViewBag.Types = _typeService.Gets();
            ViewBag.TypeId = typeId;
            return View();
        }

        [Function("STATISTIC_REPORT")]
        public object GetReports(TableModel model, int yearId = 0, int typeId = 0)
        {
            var lst = _reportService.Gets(yearId, typeId).Select(i => new { i.Id, i.Locked, i.Updated, i.Creator, i.Name, i.File, i.Desc, i.Run });
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    Updated = i.Updated.ToAppDateTime(),
                    Creator = i.Creator?.Staff?.Name,
                    i.Name,
                    i.File,
                    i.Desc,
                    i.Locked,
                    i.Run
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("STATISTIC_REPORT")]
        public ActionResult ReportEdit()
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.Departments = _departmentService.Gets(new[] { DepartmentTypes.Faculty });
            ViewBag.Types = _typeService.Gets().OrderBy(i => i.Id);
            ViewBag.Grades = _gradeService.Gets();
            return View();
        }

        [Function("STATISTIC_REPORT")]
        public JsonResult GetSemester(int yearId)
        {
            return Json(_semesterService.GetByYear(yearId), JsonRequestBehavior.AllowGet);
        }

        [Function("STATISTIC_REPORT")]
        public JsonResult GetReport(int yearId)
        {
            return Json(_reportService.GetByKey(yearId, "TOTAL_STATISTIC").OrderBy(i => i.Name).Select(i => new { i.Id, i.Name }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [Function("STATISTIC_REPORT")]
        public JsonResult GetMoneyReport(int yearId)
        {
            return Json(_reportService.GetByKey(yearId, "MONEY_STATISTIC").OrderBy(i => i.Name).Select(i => new { i.Id, i.Name }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [Function("STATISTIC_REPORT")]
        [HttpPost]
        public JsonResult ReportEdit(int yearId, int semesterId, int gradeId, int typeId, int reportId, int moneyReportId, string name, int departmentId, int staffFilter, string desc, bool facultyComplement)
        {
            var s = new ReportData
            {
                YearId = yearId,
                SemesterId = semesterId,
                GradeId = gradeId,
                TypeId = typeId,
                ReportId = reportId,
                MoneyReportId = moneyReportId,
                Name = name,
                DepartmentId = departmentId,
                StaffFilter = staffFilter,
                Desc = desc,
                CreateBy = WorkContext.UserInfo.Id,
                ApplyFacultyRatio = facultyComplement
            };
            _reportService.Insert(s);
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [Function("STATISTIC_REPORT")]
        [HttpPost]
        public void RunStatistic(int id)
        {
            _reportService.RunStatistic(id);
        }

        [Function("STATISTIC_REPORT")]
        public string ReportDownload(int id)
        {
            return _reportService.Download(id);
        }

        [Function("STATISTIC_REPORT")]
        public ActionResult ReportDelete(int id)
        {
            _reportService.Delete(id);
            return RedirectToAction("Report");
        }

        [Function("STATISTIC_REPORT_LOCK")]
        public ActionResult Lock(int yearId = 0, int typeId = 0)
        {
            var years = _yearService.Gets();
            ViewBag.Years = years;
            ViewBag.YearId = yearId > 0 ? yearId : years.FirstOrDefault()?.Id;
            ViewBag.Types = _typeService.Gets();
            ViewBag.TypeId = typeId;
            return View();
        }

        [Function("STATISTIC_REPORT_LOCK")]
        public object GetLocks(TableModel model, int yearId = 0, int typeId = 0)
        {
            var lst = _reportService.Gets(yearId, typeId).Select(i => new { i.Id, i.Locked, i.Updated, i.Creator, i.Name, i.File, i.Desc, i.Run });
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    Updated = i.Updated.ToAppDateTime(),
                    Creator = i.Creator?.Staff?.Name,
                    i.Name,
                    i.File,
                    i.Desc,
                    i.Locked,
                    i.Run
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("STATISTIC_REPORT_LOCK")]
        [HttpPost]
        public void LockToggle(int id)
        {
            _reportService.LockToggle(id);
        }
        #endregion

        #region REPORT TYPE
        [Admin]
        public ActionResult ReportType(int deId = 0)
        {
            return View();
        }

        [Admin]
        public object GetReportTypes(TableModel model)
        {
            var lst = _typeService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Admin]
        public ActionResult ReportTypeEdit(int id = 0)
        {
            var m = new ReportTypeModel { Id = id };
            if (id == 0) return View(m);
            var d = _typeService.Get(id);
            m.Name = d.Name;
            m.Template = d.Template;
            m.File = d.File;
            return View(m);
        }

        [Admin]
        [HttpPost]
        public ActionResult ReportTypeEdit(ReportTypeModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new ReportType
            {
                Name = model.Name,
                Id = model.Id,
                Template = model.Template,
                File = model.File
            };
            _typeService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Admin]
        public ActionResult ReportTypeDelete(int id)
        {
            var d = _typeService.Get(id);
            _typeService.Delete(d);
            return RedirectToAction("ReportType");
        }
        #endregion
    }
}