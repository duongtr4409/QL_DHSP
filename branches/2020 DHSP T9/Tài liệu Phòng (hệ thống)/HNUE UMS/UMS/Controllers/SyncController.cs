using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Security;
using Ums.Core.Domain.Task;
using Ums.Models.Common;
using Ums.Models.Sync;
using Ums.Services.Conversion;
using Ums.Services.Data;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Sync;

namespace Ums.Website.Controllers
{
    public class SyncController : BaseController
    {
        private readonly IYearService _yearService;
        private readonly IDepartmentService _departmentService;
        private readonly ITeachingSyncService _syncTeachingService;
        private readonly Services.Task.ITeachingService _teachingService;
        private readonly IGradeService _gradeService;
        private readonly ITeachingService _conversionTeachingService;
        private readonly IStaffService _staffService;
        private readonly IResearchingSyncService _researchingSyncService;
        private readonly IResearchingService _conversionResearching;
        private readonly Services.Task.ResearchingService _researchingService;
        public SyncController(IYearService yearService, IDepartmentService departmentService, ITeachingSyncService syncTeachingService, Services.Task.ITeachingService teachingService, IGradeService gradeService, ITeachingService conversionTeachingService, IStaffService staffService, IResearchingSyncService researchingSyncService, IResearchingService conversionResearching, Services.Task.ResearchingService researchingService)
        {
            _yearService = yearService;
            _departmentService = departmentService;
            _syncTeachingService = syncTeachingService;
            _teachingService = teachingService;
            _gradeService = gradeService;
            _conversionTeachingService = conversionTeachingService;
            _staffService = staffService;
            _researchingSyncService = researchingSyncService;
            _conversionResearching = conversionResearching;
            _researchingService = researchingService;
        }

        [Function("SYNC_STANDARD")]
        public ActionResult Standard(int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            ViewBag.Grades = _gradeService.Gets().OrderBy(i => i.Name).ToList();
            ViewBag.GradeId = gradeId;
            return View();
        }

        [Function("SYNC_STANDARD")]
        public object GetStandard(TableModel model, int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            var s = _teachingService.Gets(yearId: yearId, departmentId: departmentId, gradeId: gradeId).Where(i => !string.IsNullOrEmpty(i.SyncId)).OrderBy(i => i.SyncId);
            return s.ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    ForDepartment = i.ForDepartment?.Name,
                    i.LessonTime,
                    i.Size,
                    i.TeachingTime,
                    Lecturer = i.Invited ? i.Lecturer : i.Staff?.Name,
                    i.Paid
                }).AsQueryable().ToTableResult(s.Count(), model.Draw).ToJson();
        }

        [Function("SYNC_STANDARD")]
        public ActionResult StandardEdit(int yearId = 0, int gradeId = 0, int departmentId = 0, int conversionId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            ViewBag.Grades = _gradeService.Gets().OrderBy(i => i.Name).ToList();
            ViewBag.GradeId = gradeId;
            ViewBag.Conversions = _conversionTeachingService.Gets().OrderBy(i => i.Name).ToList();
            ViewBag.ConversionId = conversionId;
            return View(new TeachingSyncModel());
        }

        [Function("SYNC_STANDARD")]
        public JsonResult SyncStandard(int yearId = 0, int gradeId = 0, int semester = 0, int departmentId = 0, int conversionId = 0)
        {
            var departments = _departmentService.Gets().ToList();
            var conversions = _conversionTeachingService.Gets().ToList();
            var grades = _gradeService.Gets().ToList();
            var data = _syncTeachingService.GetStandard(yearId, departmentId, semester, gradeId, conversionId);
            foreach (var d in data)
            {
                d.ForDepartment = departments.FirstOrDefault(i => i.Id == d.ForDepartmentId)?.Name ?? "";
                d.Department = departments.FirstOrDefault(i => i.Id == d.DepartmentId)?.Name ?? "";
                d.Conversion = conversions.FirstOrDefault(i => i.Id == d.ConversionId)?.Name ?? "";
                d.Grade = grades.FirstOrDefault(i => i.Id == d.GradeId)?.Name ?? "";
            }
            return Json(data.OrderByDescending(i => i.IsOk), JsonRequestBehavior.AllowGet);
        }

        [Function("SYNC_STANDARD")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult StandardEdit(TeachingSyncModel model, FormCollection form)
        {
            var data = model.Data.CastJson<List<TeachingSyncTask>>().Where(i => form.GetValues("selected[]").Contains(i.SyncId));
            foreach (var d in data)
            {
                var task = _teachingService.Gets().FirstOrDefault(i => i.SyncId == d.SyncId) ?? new TaskTeaching();
                d.CopyTo(task, "Id");
                task.LessonTime = d.LessionTime;
                task.Name = "[" + d.SubjectCode + "] " + d.Name;
                task.SyncStaffId = WorkContext.UserInfo.StaffId;
                task.Size = d.Size;
                _teachingService.InsertOrUpdate(task);
            }
            return IframeScript;
        }

        [Function("SYNC_OFF_CAMPUS")]
        public ActionResult OffCampus(int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            ViewBag.Grades = _gradeService.Gets().OrderBy(i => i.Name).ToList();
            ViewBag.GradeId = gradeId;
            return View();
        }

        [Function("SYNC_OFF_CAMPUS")]
        public object GetOffCampus(TableModel model, int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            var s = _teachingService.Gets(yearId: yearId, departmentId: departmentId, gradeId: gradeId).Where(i => !string.IsNullOrEmpty(i.SyncId)).OrderBy(i => i.SyncId);
            return s.ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    ForDepartment = i.ForDepartment?.Name,
                    i.LessonTime,
                    i.Size,
                    i.TeachingTime,
                    Lecturer = i.Invited ? i.Lecturer : i.Staff?.Name,
                    i.Paid
                }).AsQueryable().ToTableResult(s.Count(), model.Draw).ToJson();
        }

        [Function("SYNC_OFF_CAMPUS")]
        public ActionResult OffCampusEdit(int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            ViewBag.Grades = _gradeService.Gets().OrderBy(i => i.Name).ToList();
            ViewBag.GradeId = gradeId;
            return View(new TeachingSyncModel());
        }

        [Function("SYNC_OFF_CAMPUS")]
        public JsonResult SyncOffCampus(int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            var departments = _departmentService.Gets().ToList();
            var conversions = _conversionTeachingService.Gets().ToList();
            var grades = _gradeService.Gets().ToList();
            var stave = _staffService.Gets().ToList();
            var data = _syncTeachingService.GetOffCampus(yearId, departmentId, gradeId);
            foreach (var d in data)
            {
                d.ForDepartment = departments.FirstOrDefault(i => i.Id == d.ForDepartmentId)?.Name ?? "";
                d.Department = departments.FirstOrDefault(i => i.Id == d.DepartmentId)?.Name ?? "";
                d.Conversion = conversions.FirstOrDefault(i => i.Id == d.ConversionId)?.Name ?? "";
                d.Grade = grades.FirstOrDefault(i => i.Id == d.GradeId)?.Name ?? "";
                var staff = stave.FirstOrDefault(i => i.Id == d.StaffId);
                d.Lecturer = staff != null ? (staff.Department != null ? "[" + staff.Department.Name + "]<br/>" : "") + staff.Name : d.InvitedLecturer;
            }
            return Json(data.OrderByDescending(i => i.IsOk), JsonRequestBehavior.AllowGet);
        }

        [Function("SYNC_OFF_CAMPUS")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult OffCampusEdit(TeachingSyncModel model, FormCollection form)
        {
            var data = model.Data.CastJson<List<TeachingSyncTask>>().Where(i => form.GetValues("selected[]").Contains(i.SyncId));
            foreach (var d in data)
            {
                var task = _teachingService.Gets().FirstOrDefault(i => i.SyncId == d.SyncId) ?? new TaskTeaching();
                d.CopyTo(task);
                task.LessonTime = d.LessionTime;
                task.Name = "[" + d.SubjectCode + "] " + d.Name;
                task.SyncStaffId = WorkContext.UserInfo.StaffId;
                task.Size = d.Size;
                _teachingService.InsertOrUpdate(task);
            }
            return IframeScript;
        }

        [Function("SYNC_POST_GRADUATE")]
        public ActionResult PostGraduated(int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            ViewBag.Grades = _gradeService.Gets().OrderBy(i => i.Name).ToList();
            ViewBag.GradeId = gradeId;
            return View();
        }

        [Function("SYNC_POST_GRADUATE")]
        public object GetPostGraduated(TableModel model, int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            var s = _teachingService.Gets(yearId: yearId, departmentId: departmentId, gradeId: gradeId).Where(i => !string.IsNullOrEmpty(i.SyncId)).OrderBy(i => i.SyncId);
            return s.ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    ForDepartment = i.ForDepartment?.Name,
                    i.LessonTime,
                    i.Size,
                    i.TeachingTime,
                    Lecturer = i.Invited ? i.Lecturer : i.Staff?.Name,
                    i.Paid
                }).AsQueryable().ToTableResult(s.Count(), model.Draw).ToJson();
        }

        [Function("SYNC_POST_GRADUATE")]
        public ActionResult PostGraduatedEdit(int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            ViewBag.Grades = _gradeService.Gets().OrderBy(i => i.Name).ToList();
            ViewBag.GradeId = gradeId;
            return View(new TeachingSyncModel());
        }

        [Function("SYNC_POST_GRADUATE")]
        public JsonResult SyncPostGraduated(int yearId = 0, int gradeId = 0, int departmentId = 0)
        {
            var departments = _departmentService.Gets().ToList();
            var conversions = _conversionTeachingService.Gets().ToList();
            var grades = _gradeService.Gets().ToList();
            var stave = _staffService.Gets().ToList();
            var data = _syncTeachingService.GetOffCampus(yearId, departmentId, gradeId);
            foreach (var d in data)
            {
                d.ForDepartment = departments.FirstOrDefault(i => i.Id == d.ForDepartmentId)?.Name ?? "";
                d.Department = departments.FirstOrDefault(i => i.Id == d.DepartmentId)?.Name ?? "";
                d.Conversion = conversions.FirstOrDefault(i => i.Id == d.ConversionId)?.Name ?? "";
                d.Grade = grades.FirstOrDefault(i => i.Id == d.GradeId)?.Name ?? "";
                var staff = stave.FirstOrDefault(i => i.Id == d.StaffId);
                d.Lecturer = staff != null ? (staff.Department != null ? "[" + staff.Department.Name + "]<br/>" : "") + staff.Name : d.InvitedLecturer;
            }
            return Json(data.OrderByDescending(i => i.IsOk), JsonRequestBehavior.AllowGet);
        }

        [Function("SYNC_POST_GRADUATE")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult PostGraduatedEdit(TeachingSyncModel model, FormCollection form)
        {
            var data = model.Data.CastJson<List<TeachingSyncTask>>().Where(i => form.GetValues("selected[]").Contains(i.SyncId));
            foreach (var d in data)
            {
                var task = _teachingService.Gets().FirstOrDefault(i => i.SyncId == d.Id.ToString()) ?? new TaskTeaching();
                d.CopyTo(task, "Id");
                task.LessonTime = d.LessionTime;
                task.Name = "[" + d.SubjectCode + "] " + d.Name;
                task.SyncStaffId = WorkContext.UserInfo.StaffId;
                task.Size = d.Size;
                task.SyncId = d.Id.ToString();
                _teachingService.InsertOrUpdate(task);
            }
            return IframeScript;
        }

        [Function("SYNC_OFF_CAMPUS", "SYNC_POST_GRADUATE")]
        public ActionResult PostGraduatedDelete(int id)
        {
            _teachingService.Delete(id);
            return RedirectToAction("OffCampus");
        }

        [Function("SYNC_RESEARCHING")]
        public ActionResult Researching(int yearId = 0, int departmentId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            return View();
        }

        [Function("SYNC_RESEARCHING")]
        public object GetResearching(TableModel model, int yearId = 0, int departmentId = 0)
        {
            var s = _researchingService.Gets(yearId: yearId, departmentId: departmentId).Where(i => i.SyncId > 0).OrderBy(i => i.SyncId);
            return s.ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Members,
                    i.Quantity,
                    i.StartYear,
                    Staff = i.Staff?.Name
                }).AsQueryable().ToTableResult(s.Count(), model.Draw).ToJson();
        }

        [Function("SYNC_RESEARCHING")]
        public ActionResult ResearchingEdit(int yearId = 0, int departmentId = 0)
        {
            ViewBag.Years = _yearService.Gets();
            ViewBag.YearId = yearId;
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            return View(new ResearchingSyncModel { YearId = yearId });
        }

        [Function("SYNC_RESEARCHING")]
        public JsonResult SyncResearching(int yearId = 0, int departmentId = 0)
        {
            var departments = _departmentService.Gets().ToList();
            var conversions = _conversionResearching.Gets().ToList();
            var stave = _staffService.Gets().ToList();
            var data = _researchingSyncService.Gets(yearId, departmentId);
            foreach (var d in data)
            {
                d.Conversion = conversions.FirstOrDefault(i => i.Id == d.ConversionId)?.Name ?? "";
                var staff = stave.FirstOrDefault(i => i.Id == d.StaffId);
                d.Staff = stave.FirstOrDefault(i => i.Id == d.StaffId)?.Name;
            }
            return Json(data.OrderByDescending(i => i.IsOk), JsonRequestBehavior.AllowGet);
        }

        [Function("SYNC_RESEARCHING")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ResearchingEdit(ResearchingSyncModel model, FormCollection form)
        {
            var ids = form.GetValues("selected[]").Select(int.Parse).ToArray();
            var data = model.Data.CastJson<List<ResearchingSyncTask>>().Where(i => ids.Contains(i.Id));
            foreach (var d in data)
            {
                var task = _researchingService.Gets().FirstOrDefault(i => i.SyncId == d.Id) ?? new TaskResearching();
                d.CopyTo(task, "Id");
                task.SyncStaffId = WorkContext.UserInfo.StaffId;
                task.SyncId = d.Id;
                task.YearId = model.YearId;
                _researchingService.InsertOrUpdate(task);
            }
            return IframeScript;
        }

        [Function("SYNC_RESEARCHING")]
        public ActionResult ResearchingDelete(int id)
        {
            _researchingService.Delete(id);
            return RedirectToAction("Researching");
        }
    }
}