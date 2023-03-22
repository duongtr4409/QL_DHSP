using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Helper;
using Ums.App.Security;
using Ums.Core.Domain.Personnel;
using Ums.Models.Common;
using Ums.Models.Personnel;
using Ums.Services.Data;
using Ums.Services.Organize;
using Ums.Services.Personnel;

namespace Ums.Website.Controllers
{
    public class PersonnelController : BaseController
    {
        private readonly IStaffService _staffService;
        private readonly IDepartmentService _departmentService;
        private readonly IPositionGroupService _groupService;
        private readonly IStaffPositionService _staffPositionService;
        private readonly IDegreeService _degreeService;
        private readonly ITitleService _titleService;
        private readonly IClassifyService _classifyService;
        private readonly IPositionService _positionService;
        private readonly IVacationService _vacationService;
        private readonly IVacationTypeService _vacationTypeService;
        private readonly IYearService _yearService;
        private readonly IStaffClassifyService _staffClassifyService;
        public PersonnelController(IStaffService staffService, IDepartmentService departmentService, IPositionGroupService groupService, IStaffPositionService staffPositionService, IDegreeService degreeService, ITitleService titleService, IClassifyService classifyService, IPositionService positionService, IVacationService vacationService, IVacationTypeService vacationTypeService, IYearService yearService, IStaffClassifyService staffClassifyService)
        {
            _staffService = staffService;
            _departmentService = departmentService;
            _groupService = groupService;
            _staffPositionService = staffPositionService;
            _degreeService = degreeService;
            _titleService = titleService;
            _classifyService = classifyService;
            _positionService = positionService;
            _vacationService = vacationService;
            _vacationTypeService = vacationTypeService;
            _yearService = yearService;
            _staffClassifyService = staffClassifyService;
        }

        #region Staff Manager
        [Function("STAFF_MANAGER")]
        public ActionResult Staff(int departmentId = 0, int titleId = 0, int moved = 0, int retired = 0, string keyword = "")
        {
            ViewBag.DepartmentId = departmentId;
            ViewBag.Departments = _departmentService.Gets().ToPair();
            ViewBag.Keyword = keyword;
            ViewBag.TitleId = titleId;
            ViewBag.Titles = _titleService.Gets().OrderBy(i => i.Name).ToPair();
            ViewBag.Moved = moved;
            ViewBag.Retired = retired;
            return View();
        }

        [Function("STAFF_MANAGER")]
        public object GetStaff(TableModel model, int departmentId = 0, int titleId = 0, int moved = 0, int retired = 0, string keyword = "")
        {
            var lst = _staffService.Gets(departmentId, moved, retired, keyword, titleId);
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.Gender,
                    i.StartYear,
                    Degree = i.Degree?.Name,
                    Title = i.Title?.Name,
                    i.Code,
                    i.IsRetired,
                    i.IsMoved,
                    RMDate = i.RetireOrMoveDate.ToAppDate(),
                    Positions = i.Positions?.Aggregate("<ol>", (c, j) => c + "<li>" + j.Position.Name + "</li>") + "</ol>",
                    Birthday = i.Birthday.ToAppDate()
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("STAFF_MANAGER")]
        public ActionResult StaffEdit(int id = 0)
        {
            var levels = new List<KeyValuePair<int, int>>();
            for (var i = 1; i < 13; i++)
            {
                levels.Add(new KeyValuePair<int, int>(i, i));
            }
            var departments = _departmentService.Gets().Select(i => new { i.Id, i.Name }).OrderBy(i => i.Name).ToList();
            departments.Insert(0, new { Id = 0, Name = "---Chọn---" });
            var m = new StaffModel
            {
                Departments = new SelectList(departments, "Id", "Name"),
                Degrees = new SelectList(_degreeService.Gets().Select(i => new { i.Id, i.Name }).OrderBy(i => i.Name).ToList(), "Id", "Name"),
                Titles = new SelectList(_titleService.Gets().Select(i => new { i.Id, i.Name }).OrderBy(i => i.Name).ToList(), "Id", "Name"),
                Genders = new SelectList(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Nam","Nam"),
                    new KeyValuePair<string, string>("Nữ","Nữ")
                }, "Value", "Key"),
                Levels = new SelectList(levels, "Value", "Key"),
                Id = id
            };
            if (id == 0) return View(m);
            var d = _staffService.Get(id);
            d.CopyTo(m);
            m.Level = d.SalaryLevel;
            m.TeachingInId = d.TeachingInId == 0 ? d.DepartmentId : d.TeachingInId;
            m.SalaryRaiseOn = d.SalaryRaiseOn.ToAppDate();
            m.RetireOrMoveDate = d.RetireOrMoveDate.ToAppDate();
            m.Birthday = d.Birthday.ToAppDate();
            return View(m);
        }

        [Function("STAFF_MANAGER")]
        [HttpPost]
        public ActionResult StaffEdit(StaffModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = _staffService.Get(model.Id) ?? new Staff();
            model.CopyTo(d);
            d.SalaryLevel = model.Level;
            d.SalaryRaiseOn = model.SalaryRaiseOn.ToAppDate();
            d.RetireOrMoveDate = model.RetireOrMoveDate.ToAppDate();
            d.Birthday = model.Birthday.ToAppDate();
            _staffService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("STAFF_MANAGER")]
        public ActionResult StaffDelete(int id)
        {
            var d = _staffService.Get(id);
            _staffService.Delete(d);
            return RedirectToAction("Staff", new { deId = d.DepartmentId });
        }
        #endregion

        #region Staff Indexing
        [Function("STAFF_INDEXING")]
        public ActionResult Indexing(int yearId = 0, int departmentId = 0, int titleId = 0, int moved = 0, int retired = 0, string keyword = "")
        {
            var years = _yearService.Gets();
            ViewBag.Years = years;
            ViewBag.YearId = yearId > 0 ? yearId : years.FirstOrDefault()?.Id;
            ViewBag.DepartmentId = departmentId;
            ViewBag.Departments = _departmentService.Gets().ToPair();
            ViewBag.Keyword = keyword;
            ViewBag.TitleId = titleId;
            ViewBag.Titles = _titleService.Gets().OrderBy(i => i.Name).ToPair();
            var classes = _classifyService.Gets().ToList();
            ViewBag.Classes = classes;
            ViewBag.Classifies = classes.ToJson();
            ViewBag.Moved = moved;
            ViewBag.Retired = retired;
            return View();
        }

        [Function("STAFF_INDEXING")]
        public object GetIndexing(TableModel model, int yearId = 0, int departmentId = 0, int titleId = 0, int moved = 0, int retired = 0, string keyword = "")
        {
            var lst = _staffService.Gets(departmentId, titleId: titleId, keyword: keyword, moved: moved, retired: retired);
            if (moved == 1 || retired == 1)
            {
                var year = _yearService.Get(yearId);
                lst = lst.Where(i => i.RetireOrMoveDate >= year.From);
                lst = lst.Where(i => i.RetireOrMoveDate <= year.To);
            }
            lst = lst.OrderBy(model.Order);
            return (from i in lst.Skip(model.Start).Take(model.Pagesize).ToList()
                    let sf = _staffClassifyService.GetClassify(yearId, i.Id)
                    select new
                    {
                        i.Id,
                        i.Name,
                        i.Gender,
                        i.StartYear,
                        Degree = i.Degree?.Name,
                        Title = i.Title?.Name,
                        i.Code,
                        i.IsRetired,
                        i.IsMoved,
                        i.RetireOrMoveDate,
                        Positions = i.Positions?.Aggregate("<ol>", (c, j) => c + "<li>" + j.Position.Name + "</li>") + "</ol>",
                        Classify = sf?.Classify?.Name,
                        sf?.ClassifyId,
                        IndexedBy = sf?.Indexer?.Name,
                        IndexedOn = sf?.IndexedOn.ToAppDateTime(),
                        Locked = sf?.IsLocked
                    }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("STAFF_INDEXING")]
        [HttpPost]
        public void UpdateIndexing(int yearId, int staffId, int classId)
        {
            _staffClassifyService.UpdateStaffClassify(yearId, staffId, classId);
        }

        [Function("STAFF_INDEXING")]
        [HttpPost]
        public void SetDefault(int yearId = 0, int departmentId = 0, int titleId = 0, int classId = 0, string keyword = "")
        {
            var staved = _staffService
                .Gets(departmentId, titleId: titleId, keyword: keyword, moved: 2, retired: 2)
                .Select(i => i.Id).ToArray();
            foreach (var id in staved)
            {
                _staffClassifyService.UpdateStaffClassify(yearId, id, classId);
            }
        }
        #endregion

        #region Position Manager
        [Function("STAFF_POSITION")]
        public ActionResult Position(int departmentId = 0, int staffId = 0)
        {
            ViewBag.DepartmentId = departmentId;
            ViewBag.StaffId = staffId;
            ViewBag.Departments = _departmentService.Gets().Select(i => new IdNameModel { Id = i.Id, Name = i.Name }).OrderBy(i => i.Name).ToList();
            ViewBag.Staves = _staffService.Gets(departmentId).Select(i => new IdNameModel { Id = i.Id, Name = i.Name + " [Mã CB:..." + i.Code.Substring(9) + "]" }).OrderBy(i => i.Name).ToList();
            return View();
        }

        [Function("STAFF_POSITION")]
        public object GetPositions(TableModel model, int departmentId = 0, int staffId = 0)
        {
            var lst = _staffPositionService.Gets();
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.Staff.DepartmentId == departmentId);
            }
            if (staffId > 0)
            {
                lst = lst.Where(i => i.StaffId == staffId);
            }
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    Department = i.Staff?.Department?.Name ?? "[Không xác định]",
                    Staff = i.Staff?.Name ?? "[Không xác định]",
                    Position = i.Position?.Group?.Name + "<br/><b>" + i.Position?.Name + "</b>",
                    Start = i.Start.ToAppDate(),
                    End = i.End.ToAppDate(),
                    Updater = i.Updateby?.Staff?.Name + "<br/>" + i.Updated.ToAppDateTime(),
                    i.Position?.Quota
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("STAFF_POSITION")]
        public ActionResult PositionEdit(int id = 0, int departmentId = 0, int staffId = 0)
        {
            ViewBag.Categories = _groupService.GetTree().Select(i => new IdNameModel { Id = i.Id, Name = i.Name }).ToList();
            ViewBag.Departments = _departmentService.Gets().Select(i => new IdNameModel { Id = i.Id, Name = i.Name }).OrderBy(i => i.Name).ToList();
            if (id == 0)
            {
                return View(new StaffPositionModel
                {
                    DepartmentId = departmentId,
                    StaffId = staffId
                });
            }
            var s = _staffPositionService.Get(id);
            var m = new StaffPositionModel();
            s.CopyTo(m, nameof(StaffPosition.Position));
            m.DepartmentId = s.Staff?.DepartmentId ?? 0;
            m.CategoryId = s.Position?.GroupId ?? 0;
            m.Start = s.Start.ToAppDate();
            m.End = s.End.ToAppDate();
            return View(m);
        }

        [Function("STAFF_POSITION", "STAFF_VACATION")]
        public JsonResult GetStaves(int departmentId)
        {
            return Json(_staffService.Gets(departmentId)
                 .OrderBy(i => i.Name)
                 .Select(i => new
                 {
                     i.Id,
                     Name = i.Name + " [Mã CB:..." + i.Code.Substring(9) + "]",
                     Degree = i.Degree.Name
                 }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [Function("STAFF_POSITION")]
        public JsonResult GetPositionData(int categoryId)
        {
            return Json(_positionService.Gets(categoryId).OrderBy(i => i.Name)
                 .Select(i => new
                 {
                     i.Id,
                     i.Name
                 }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [Function("STAFF_POSITION")]
        [HttpPost]
        public ActionResult PositionEdit(StaffPositionModel model)
        {
            var d = _staffPositionService.Get(model.Id) ?? new StaffPosition();
            d.StaffId = model.StaffId;
            d.PositionId = model.PositionId;
            d.Start = model.Start.ToAppDate();
            d.End = model.End.ToAppDate();
            d.UpdatebyId = WorkContext.UserInfo.Id;
            d.Updated = DateTime.Now;
            _staffPositionService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("STAFF_POSITION")]
        public ActionResult PositionDelete(int id)
        {
            var d = _staffPositionService.Get(id);
            _staffPositionService.Delete(id);
            return RedirectToAction("Position", new { departmentId = d.Staff?.DepartmentId });
        }
        #endregion

        #region Vacation
        [Function("STAFF_VACATION")]
        public ActionResult Vacation(int departmentId = 0)
        {
            ViewBag.DepartmentId = departmentId;
            ViewBag.Departments = _departmentService.Gets().ToPair();
            return View();
        }

        [Function("STAFF_VACATION")]
        public object GetVacation(TableModel model, int departmentId = 0)
        {
            var lst = _vacationService.Gets();
            if (departmentId > 0)
            {
                lst = lst.Where(i => i.Staff.DepartmentId == departmentId || i.Staff.TeachingInId == departmentId);
            }
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Type.Name,
                    Staff = "<b>" + i.Staff.Degree?.Name + " " + i.Staff.Name + "</b><br/>" + i.Staff.Code,
                    StartYear = i.Staff.StartMonth + "/" + i.Staff.StartYear,
                    Start = i.Start.ToString("dd/MM/yyyy"),
                    End = i.End.ToString("dd/MM/yyyy"),
                    i.Days
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("STAFF_VACATION")]
        public ActionResult VacationEdit(int id = 0, int departmentId = 0)
        {
            ViewBag.Departments = _departmentService.Gets().Select(i => new IdNameModel { Id = i.Id, Name = i.Name }).OrderBy(i => i.Name).ToList();
            ViewBag.Types = _vacationTypeService.Gets().Select(i => new IdNameModel { Id = i.Id, Name = i.Name }).OrderBy(i => i.Name).ToList();
            var m = new VacationModel { Id = id, DepartmentId = departmentId };
            if (id == 0) return View(m);
            var d = _vacationService.Get(id);
            m.TypeId = d.TypeId;
            m.DepartmentId = d.Staff.DepartmentId;
            m.Start = d.Start.ToAppDate();
            m.End = d.End.ToAppDate();
            m.StaffId = d.StaffId;
            return View(m);
        }

        [Function("STAFF_VACATION")]
        [HttpPost]
        public ActionResult VacationEdit(VacationModel model)
        {
            var d = new Vacation
            {
                TypeId = model.TypeId,
                Id = model.Id,
                StaffId = model.StaffId,
                Start = model.Start.ToAppDate(),
                End = model.End.ToAppDate(),
                Updated = DateTime.Now,
                UpdateBy = WorkContext.UserInfo.Id
            };
            _vacationService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("STAFF_VACATION")]
        public ActionResult VacationDelete(int id)
        {
            var d = _vacationService.Get(id);
            _vacationService.Delete(d);
            return RedirectToAction("Vacation");
        }
        #endregion
    }
}
