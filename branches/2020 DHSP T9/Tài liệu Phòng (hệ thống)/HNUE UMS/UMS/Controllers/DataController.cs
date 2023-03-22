using System;
using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Helper;
using Ums.App.Security;
using Ums.Core.Domain.Data;
using Ums.Core.Types;
using Ums.Models.Common;
using Ums.Models.Data;
using Ums.Services.Data;

namespace Ums.Website.Controllers
{
    public class DataController : BaseController
    {
        private readonly IGradeService _gradeService;
        private readonly IYearService _yearService;
        private readonly ISemesterService _semesterService;
        private readonly ITitleService _titleService;
        private readonly ITitleTypeService _titleTypeService;
        private readonly IClassifyService _classifyService;
        private readonly IPositionService _positionService;
        private readonly IPositionGroupService _groupService;
        private readonly IDegreeService _degreeService;
        public DataController(IGradeService gradeService, IYearService yearService, ISemesterService semesterService, ITitleService titleService, ITitleTypeService titleTypeService, IClassifyService classifyService, IPositionService positionService, IPositionGroupService groupService, IDegreeService degreeService)
        {
            _gradeService = gradeService;
            _yearService = yearService;
            _semesterService = semesterService;
            _titleService = titleService;
            _titleTypeService = titleTypeService;
            _classifyService = classifyService;
            _positionService = positionService;
            _groupService = groupService;
            _degreeService = degreeService;
        }

        #region DEGREE
        [Function("DATA_DEGREE")]
        public ActionResult Degree(int deId = 0)
        {
            return View();
        }

        [Function("DATA_DEGREE")]
        public object GetDegree(TableModel model)
        {
            var lst = _degreeService.Gets().OrderBy(i => i.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_DEGREE")]
        public ActionResult DegreeEdit(int deId = 0, int id = 0)
        {
            var m = new DegreeModel();
            if (id == 0) return View(m);
            var d = _degreeService.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.Order = d.Order;
            m.Ratio = d.Ratio;
            return View(m);
        }

        [Function("DATA_DEGREE")]
        [HttpPost]
        public ActionResult DegreeEdit(DegreeModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new Degree
            {
                Name = model.Name,
                Order = model.Order,
                Id = model.Id,
                Ratio = model.Ratio
            };
            _degreeService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("DATA_DEGREE")]
        public ActionResult DegreeDelete(int id)
        {
            var d = _degreeService.Get(id);
            _degreeService.Delete(d);
            return RedirectToAction("Degree");
        }

        [Function("DATA_DEGREE")]
        public ActionResult DegreeSort(int[] ids)
        {
            var index = 1;
            foreach (var p in ids.Select(i => _degreeService.Get(i)))
            {
                p.Order = index++;
                _degreeService.Update(p);
            }
            return Json(ids);
        }
        #endregion

        #region POSITION
        [Function("DATA_POSITION")]
        public ActionResult Position(int groupId = 0)
        {
            ViewBag.Groups = _groupService.GetTree();
            ViewBag.GroupId = groupId;
            return View();
        }

        [Function("DATA_POSITION")]
        public object GetPosition(TableModel model, int groupId)
        {
            var lst = _positionService.Gets(groupId)
                .Select(i => new
                {
                    i.Name,
                    i.Allowance,
                    i.Desc,
                    i.Quota,
                    i.Ratio,
                    Group = i.Group.Name,
                    i.Id,
                    i.IsPublic
                });
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_POSITION")]
        public ActionResult PositionEdit(int categoryId = 0, int id = 0)
        {
            var lst = _groupService.GetTree();
            var m = new PositionModel
            {
                CategoryId = categoryId,
                Categories = new SelectList(lst, "Id", "Name")
            };
            if (id == 0) return View(m);
            var d = _positionService.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.Allowance = d.Allowance;
            m.Desc = d.Desc;
            m.Quota = d.Quota;
            m.Ratio = d.Ratio;
            m.CategoryId = d.GroupId;
            m.IsPublic = d.IsPublic;
            return View(m);
        }

        [Function("DATA_POSITION")]
        [HttpPost]
        public ActionResult PositionEdit(PositionModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new Position
            {
                Name = model.Name,
                Id = model.Id,
                GroupId = model.CategoryId,
                Allowance = model.Allowance,
                Desc = model.Desc,
                Quota = model.Quota,
                Ratio = model.Ratio,
                IsPublic = model.IsPublic
            };
            _positionService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("DATA_POSITION")]
        public ActionResult PositionDelete(int id)
        {
            var d = _positionService.Get(id);
            _positionService.Delete(d);
            return RedirectToAction("Position");
        }
        #endregion

        #region POSITION CATEGORY
        [Function("DATA_POSITION_GROUP")]
        public ActionResult PositionGroup()
        {
            return View();
        }

        [Function("DATA_POSITION_GROUP")]
        public object GetPositionGroup(TableModel model)
        {
            var lst = _groupService.Gets();
            lst = lst.OrderBy(i => model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_POSITION_GROUP")]
        public ActionResult PositionGroupEdit(int deId = 0, int id = 0)
        {
            var m = new PositionGroupModel();
            if (id == 0) return View(m);
            var d = _groupService.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            return View(m);
        }

        [Function("DATA_POSITION_GROUP")]
        [HttpPost]
        public ActionResult PositionGroupEdit(PositionGroupModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var c = _groupService.Get(model.Id) ?? new PositionGroup();
            c.Name = model.Name;
            _groupService.InsertOrUpdate(c);
            return IframeScript;
        }

        [Function("DATA_POSITION_GROUP")]
        public ActionResult PositionGroupDelete(int id)
        {
            _groupService.Delete(id);
            return RedirectToAction("PositionGroup");
        }
        #endregion

        #region CLASSIFY
        [Function("DATA_CLASSIFY")]
        public ActionResult Classify()
        {
            return View();
        }

        [Function("DATA_CLASSIFY")]
        public object GetClassify(TableModel model)
        {
            var lst = _classifyService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_CLASSIFY")]
        public ActionResult ClassifyEdit(int id = 0)
        {
            var m = new ClassifyModel();
            if (id == 0) return View(m);
            var d = _classifyService.Get(id);
            m.Name = d.Name;
            m.Ratio = d.Ratio;
            return View(m);
        }

        [Function("DATA_CLASSIFY")]
        [HttpPost]
        public ActionResult ClassifyEdit(ClassifyModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new Classify
            {
                Name = model.Name,
                Id = model.Id,
                Ratio = model.Ratio
            };
            _classifyService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("DATA_CLASSIFY")]
        public ActionResult ClassifyDelete(int id)
        {
            var d = _classifyService.Get(id);
            _classifyService.Delete(d);
            return RedirectToAction("Classify");
        }
        #endregion

        #region TITLE
        [Function("DATA_TITLE")]
        public ActionResult Title(int deId = 0)
        {
            return View();
        }

        [Function("DATA_TITLE")]
        public object GetTitle(TableModel model)
        {
            var lst = _titleService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize)
                .Select(i => new
                {
                    i.Id,
                    i.Code,
                    i.Name,
                    i.Level1,
                    i.Level2,
                    i.Level3,
                    i.Level4,
                    i.Level5,
                    i.Level6,
                    i.Level7,
                    i.Level8,
                    i.Level9,
                    i.Level10,
                    i.Level11,
                    i.Level12
                }).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_TITLE")]
        public ActionResult TitleEdit(int deId = 0, int id = 0)
        {
            var lst = Enum.GetNames(typeof(TitleTypes)).Select(i => new { Text = i, Value = (int)((TitleTypes)Enum.Parse(typeof(TitleTypes), i)) });
            var m = new TitleModel
            {
                TitleTypes = new SelectList(lst, "Value", "Text")
            };
            if (id == 0) return View(m);
            var d = _titleService.Get(id);
            m.Name = d.Name;
            m.ShortName = d.ShortName;
            m.Id = d.Id;
            m.Code = d.Code;
            m.Level1 = d.Level1;
            m.Level2 = d.Level2;
            m.Level3 = d.Level3;
            m.Level4 = d.Level4;
            m.Level5 = d.Level5;
            m.Level6 = d.Level6;
            m.Level7 = d.Level7;
            m.Level8 = d.Level8;
            m.Level9 = d.Level9;
            m.Level10 = d.Level10;
            m.Level11 = d.Level11;
            m.Level12 = d.Level12;
            m.TitleTypeId = d.TitleTypeId;
            return View(m);
        }

        [Function("DATA_TITLE")]
        [HttpPost]
        public ActionResult TitleEdit(TitleModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new Title
            {
                Name = model.Name,
                ShortName = model.ShortName,
                Id = model.Id,
                Code = model.Code,
                TitleTypeId = model.TitleTypeId,
                Level1 = model.Level1,
                Level2 = model.Level2,
                Level3 = model.Level3,
                Level4 = model.Level4,
                Level5 = model.Level5,
                Level6 = model.Level6,
                Level7 = model.Level7,
                Level8 = model.Level8,
                Level9 = model.Level9,
                Level10 = model.Level10,
                Level11 = model.Level11,
                Level12 = model.Level12
            };
            _titleService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("DATA_TITLE")]
        public ActionResult TitleDelete(int id)
        {
            var d = _titleService.Get(id);
            _titleService.Delete(d);
            return RedirectToAction("Title");
        }
        #endregion

        #region TITLE TYPE
        [Function("DATA_TITLE_TYPE")]
        public ActionResult TitleType()
        {
            return View();
        }

        [Function("DATA_TITLE_TYPE")]
        public object GetTitleType(TableModel model)
        {
            var lst = _titleTypeService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_TITLE_TYPE")]
        public ActionResult TitleTypeEdit(int id = 0)
        {
            var m = new TitleTypeModel();
            if (id == 0) return View(m);
            var tt = _titleTypeService.Get(id);
            tt.CopyTo(m);
            return View(m);
        }

        [Function("DATA_TITLE_TYPE")]
        [HttpPost]
        public ActionResult TitleTypeEdit(TitleTypeModel model)
        {
            var tt = _titleTypeService.Get(model.Id) ?? new TitleType();
            model.CopyTo(tt);
            _titleTypeService.InsertOrUpdate(tt);
            return IframeScript;
        }

        [Function("DATA_TITLE_TYPE")]
        public ActionResult TitleTypeDelete(int id)
        {
            _titleTypeService.Delete(id);
            return RedirectToAction("TitleType");
        }
        #endregion

        #region SEMESTER 
        [Function("DATA_SEMESTER")]
        public ActionResult Semester()
        {
            return View();
        }

        [Function("DATA_SEMESTER")]
        public object GetSemester(TableModel model)
        {
            var lst = _semesterService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    i.From,
                    i.To,
                    Year = i.Year?.Name
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_SEMESTER")]
        public ActionResult SemesterEdit(int id = 0)
        {
            var m = new SemesterModel();
            ViewBag.Years = _yearService.Gets();
            if (id == 0) return View(m);
            var d = _semesterService.Get(id);
            m.From = d.From.ToAppDate();
            m.To = d.To.ToAppDate();
            m.Id = d.Id;
            m.Name = d.Name;
            m.Desc = d.Desc;
            m.YearId = d.YearId;
            return View(m);
        }

        [Function("DATA_SEMESTER")]
        [HttpPost]
        public ActionResult SemesterEdit(SemesterModel model)
        {
            var d = new Semester
            {
                From = model.From.ToAppDate(),
                To = model.To.ToAppDate(),
                Id = model.Id,
                Name = model.Name,
                Desc = model.Desc,
                YearId = model.YearId
            };
            _semesterService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("DATA_SEMESTER")]
        public ActionResult SemesterDelete(int id)
        {
            _semesterService.Delete(id);
            return RedirectToAction("Semester");
        }
        #endregion

        #region YEAR
        [Function("DATA_YEAR")]
        public ActionResult Year()
        {
            return View();
        }

        [Function("DATA_YEAR")]
        public object GetYear(TableModel model)
        {
            var lst = _yearService.GetAll();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_YEAR")]
        public ActionResult YearEdit(int id = 0)
        {
            var m = new YearModel();
            if (id == 0) return View(m);
            var d = _yearService.Get(id);
            m.From = d.From.ToAppDate();
            m.To = d.To.ToAppDate();
            m.Id = d.Id;
            m.Visible = d.Visible;
            return View(m);
        }

        [Function("DATA_YEAR")]
        [HttpPost]
        public ActionResult YearEdit(YearModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new Year
            {
                From = model.From.ToAppDate(),
                To = model.To.ToAppDate(),
                Id = model.Id,
                Visible = model.Visible
            };
            _yearService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("DATA_YEAR")]
        public ActionResult YearDelete(int id)
        {
            var d = _yearService.Get(id);
            _yearService.Delete(d);
            return RedirectToAction("Year");
        }
        #endregion

        #region GRADE
        [Function("DATA_GRADE")]
        public ActionResult Grade()
        {
            return View();
        }

        [Function("DATA_GRADE")]
        public object GetGrade(TableModel model)
        {
            var lst = _gradeService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("DATA_GRADE")]
        public ActionResult GradeEdit(int id = 0)
        {
            var m = new GradeModel();
            if (id == 0) return View(m);
            var d = _gradeService.Get(id);
            d.CopyTo(m);
            return View(m);
        }

        [Function("DATA_GRADE")]
        [HttpPost]
        public ActionResult GradeEdit(GradeModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new Grade();
            model.CopyTo(d);
            _gradeService.InsertOrUpdate(d);
            return IframeScript;
        }

        [Function("DATA_GRADE")]
        public ActionResult GradeDelete(int id)
        {
            var d = _gradeService.Get(id);
            _gradeService.Delete(d);
            return RedirectToAction("Grade");
        }
        #endregion
    }
}
