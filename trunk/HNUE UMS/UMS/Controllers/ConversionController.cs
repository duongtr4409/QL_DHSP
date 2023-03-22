using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Helper;
using Ums.App.Security;
using Ums.Core.Domain.Conversion;
using Ums.Models.Common;
using Ums.Models.Conversion;
using Ums.Models.System;
using Ums.Services.Conversion;
using Ums.Services.Conversion.Type;
using Ums.Services.Data;

namespace Ums.Website.Controllers
{
    [Function("CONVERSION")]
    public class ConversionController : BaseController
    {
        private readonly IResearchingService _researchingService;
        private readonly Ums.Services.Conversion.Category.IResearchingService _researchingCategory;
        private readonly ITeachingService _teachingService;
        private readonly Ums.Services.Conversion.Category.ITeachingService _teachingCategory;
        private readonly IWorkingService _workingService;
        private readonly Ums.Services.Conversion.Category.IWorkingService _workingCategory;
        private readonly IStandardService _standardService;
        private readonly ITitleService _titleService;
        private readonly ITeachingTypeService _teachingTypeService;
        public ConversionController(IResearchingService researchingService, Ums.Services.Conversion.Category.IResearchingService researchingCategory, ITeachingService teachingService, Ums.Services.Conversion.Category.ITeachingService teachingCategory, IWorkingService workingService, Ums.Services.Conversion.Category.IWorkingService workingCategory, IStandardService standardService, ITitleService titleService, ITeachingTypeService teachingTypeService)
        {
            _researchingService = researchingService;
            _researchingCategory = researchingCategory;
            _teachingService = teachingService;
            _teachingCategory = teachingCategory;
            _workingService = workingService;
            _workingCategory = workingCategory;
            _standardService = standardService;
            _titleService = titleService;
            _teachingTypeService = teachingTypeService;
        }

        #region STANDARD
        public ActionResult Standard()
        {
            return View();
        }

        public object GetStandard(TableModel model)
        {
            var lst = _standardService.Gets().OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Researching,
                    i.Working,
                    i.Teaching,
                    i.Title.Name
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult StandardEdit(int deId = 0, int id = 0)
        {
            ViewBag.Titles = _titleService.Gets().OrderBy(i => i.Name).ToPair();
            var m = new StandardModel();
            if (id == 0) return View(m);
            var d = _standardService.Get(id);
            d.CopyTo(m);
            return View(m);
        }

        [HttpPost]
        public ActionResult StandardEdit(StandardModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new ConversionStandard();
            model.CopyTo(d);
            _standardService.InsertOrUpdate(d);
            return RedirectToAction("Standard");
        }

        public ActionResult StandardDelete(int id)
        {
            var d = _standardService.Get(id);
            _standardService.Delete(d);
            return RedirectToAction("Standard");
        }
        #endregion

        #region CONVERSION RESEARCHING
        public ActionResult Researching(int categoryId = 0)
        {
            ViewBag.Categories = _researchingCategory.GetTree();
            ViewBag.CategoryId = categoryId;
            return View();
        }

        public object GetResearching(TableModel model, int categoryId = 0)
        {
            var lst = _researchingService.Gets(categoryId);
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    Category = i.Category?.Name,
                    i.Ratio,
                    i.Factor,
                    i.Unit,
                    i.Code,
                    i.MemberOffset,
                    i.HasMember,
                    i.EquivalentQuantity
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult ResearchingEdit(int id = 0)
        {
            var m = new ResearchingModel
            {
                Categories = new SelectList(_researchingCategory.GetTree(), "Id", "Name")
            };
            if (id == 0) return View(m);
            var d = _researchingService.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.CategoryId = d.CategoryId;
            m.IsDirectly = d.IsDirectly;
            m.Ratio = d.Ratio;
            m.Unit = d.Unit;
            m.Factor = d.Factor;
            m.MaxMaterialTime = d.MaxMaterialTime;
            m.MemberOffset = d.MemberOffset;
            m.Code = d.Code;
            m.Factor = d.Factor;
            m.HasMember = d.HasMember;
            m.EquivalentQuantity = d.EquivalentQuantity;
            return View(m);
        }

        [HttpPost]
        public ActionResult ResearchingEdit(ResearchingModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new ConversionResearching
            {
                Name = model.Name,
                Id = model.Id,
                CategoryId = model.CategoryId,
                Ratio = model.Ratio,
                Unit = model.Unit,
                Factor = model.Factor,
                IsDirectly = model.IsDirectly,
                MaxMaterialTime = model.MaxMaterialTime,
                MemberOffset = model.MemberOffset,
                Code = model.Code,
                HasMember = model.HasMember,
                EquivalentQuantity = model.EquivalentQuantity
            };
            if (d.EquivalentQuantity < 1)
            {
                d.EquivalentQuantity = 1;
            }
            _researchingService.InsertOrUpdate(d);
            return IframeScript;
        }

        public ActionResult ResearchingDelete(int id)
        {
            var d = _researchingService.Get(id);
            _researchingService.Delete(d);
            return RedirectToAction("Researching");
        }
        #endregion

        #region RESEARCHING CATEGORY

        public ActionResult ResearchingCategory(int parentId = 0)
        {
            ViewBag.ParentId = parentId;
            return View();
        }

        public object GetResearchingCategories(TableModel model, int parentId = 0)
        {
            var lst = _researchingCategory.Gets().Where(i => i.ParentId == parentId);
            lst = lst.OrderBy(i => model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult ResearchingCategoryEdit(int deId = 0, int id = 0)
        {
            var m = new Ums.Models.Conversion.Category.ResearchingModel();
            ViewBag.Parents = _researchingCategory.GetTree();
            if (id == 0) return View(m);
            var d = _researchingCategory.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.ParentId = d.ParentId;
            return View(m);
        }

        [HttpPost]
        public ActionResult ResearchingCategoryEdit(Ums.Models.Conversion.Category.ResearchingModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var c = _researchingCategory.Get(model.Id) ?? new ConversionResearchingCategory();
            c.Name = model.Name;
            c.ParentId = model.ParentId;
            _researchingCategory.InsertOrUpdate(c);
            return IframeScript;
        }

        public ActionResult ResearchingCategoryDelete(int id)
        {
            _researchingCategory.Delete(id);
            return RedirectToAction("ResearchingCategory");
        }
        #endregion

        #region CONVERSION TEACHING
        public ActionResult Teaching()
        {
            return View();
        }

        public object GetTeaching(TableModel model)
        {
            var lst = _teachingService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    Category = i.Category.Name,
                    i.Desc,
                    Ratio = i.Ratio + " * " + i.Unit,
                    Type = i.Type?.Name,
                    i.GroupSize
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult TeachingEdit(int id = 0)
        {
            var lst = _teachingCategory.GetTree();
            var lst2 = _teachingTypeService.Gets().Select(i => new { Text = i.Name, Value = i.Id });
            var m = new TeachingModel
            {
                Categories = new SelectList(lst, "Id", "Name"),
                TeachingTypes = new SelectList(lst2, "Value", "Text")
            };
            if (id == 0) return View(m);
            var d = _teachingService.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.CategoryId = d.CategoryId;
            m.Desc = d.Desc;
            m.Ratio = d.Ratio;
            m.Unit = d.Unit;
            m.TeachingTypeId = d.TeachingTypeId;
            m.GroupSize = d.GroupSize;
            return View(m);
        }

        [HttpPost]
        public ActionResult TeachingEdit(TeachingModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new ConversionTeaching
            {
                Name = model.Name,
                Id = model.Id,
                CategoryId = model.CategoryId,
                Desc = model.Desc,
                Ratio = model.Ratio,
                Unit = model.Unit,
                TeachingTypeId = model.TeachingTypeId,
                GroupSize = model.GroupSize
            };
            _teachingService.InsertOrUpdate(d);
            return IframeScript;
        }

        public ActionResult TeachingDelete(int id)
        {
            _teachingService.Delete(id);
            return RedirectToAction("Teaching");
        }
        #endregion

        #region TEACHING CATEGORY

        public ActionResult TeachingCategory(int parentId = 0)
        {
            ViewBag.ParentId = parentId;
            return View();
        }

        public object GetTeachingCategories(TableModel model, int parentId = 0)
        {
            var lst = _teachingCategory.Gets().Where(i => i.ParentId == parentId);
            lst = lst.OrderBy(i => model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult TeachingCategoryEdit(int deId = 0, int id = 0)
        {
            var m = new Ums.Models.Conversion.Category.TeachingModel();
            ViewBag.Parents = _teachingCategory.GetTree();
            if (id == 0) return View(m);
            var d = _teachingCategory.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.ParentId = d.ParentId;
            return View(m);
        }

        [HttpPost]
        public ActionResult TeachingCategoryEdit(Ums.Models.Conversion.Category.TeachingModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var c = _teachingCategory.Get(model.Id) ?? new ConversionTeachingCategory();
            c.Name = model.Name;
            c.ParentId = model.ParentId;
            _teachingCategory.InsertOrUpdate(c);
            return IframeScript;
        }

        public ActionResult TeachingCategoryDelete(int id)
        {
            _teachingCategory.Delete(id);
            return RedirectToAction("TeachingCategory");
        }
        #endregion

        #region CONVERSION WORKING
        public ActionResult Working()
        {
            return View();
        }

        public object GetWorking(TableModel model)
        {
            var lst = _workingService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    Category = i.Category.Name,
                    i.Ratio,
                    i.Amount,
                    i.Unit,
                    i.Desc
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult WorkingEdit(int id = 0)
        {
            var lst = _workingCategory.GetTree();
            var m = new WorkingModel
            {
                Categories = new SelectList(lst, "Id", "Name")
            };
            if (id == 0) return View(m);
            var d = _workingService.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.CategoryId = d.CategoryId;
            m.Amount = d.Amount;
            m.Ratio = d.Ratio;
            m.Unit = d.Unit;
            m.Desc = d.Desc;
            return View(m);
        }

        [HttpPost]
        public ActionResult WorkingEdit(WorkingModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new ConversionWorking
            {
                Name = model.Name,
                Id = model.Id,
                CategoryId = model.CategoryId,
                Ratio = model.Ratio,
                Unit = model.Unit,
                Desc = model.Desc,
                Amount = model.Amount
            };
            _workingService.InsertOrUpdate(d);
            return IframeScript;
        }

        public ActionResult WorkingDelete(int id)
        {
            _workingService.Delete(id);
            return RedirectToAction("Working");
        }

        #endregion

        #region WORKING CATEGORY
        public ActionResult WorkingCategory(int parentId = 0)
        {
            ViewBag.ParentId = parentId;
            return View();
        }

        public object GetWorkingCategories(TableModel model, int parentId = 0)
        {
            var lst = _workingCategory.Gets().Where(i => i.ParentId == parentId);
            lst = lst.OrderBy(i => model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult WorkingCategoryEdit(int deId = 0, int id = 0)
        {
            var m = new Ums.Models.Conversion.Category.WorkingModel();
            ViewBag.Parents = _workingCategory.GetTree();
            if (id == 0) return View(m);
            var d = _workingCategory.Get(id);
            m.Name = d.Name;
            m.Id = d.Id;
            m.ParentId = d.ParentId;
            return View(m);
        }

        [HttpPost]
        public ActionResult WorkingCategoryEdit(Ums.Models.Conversion.Category.WorkingModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var c = _workingCategory.Get(model.Id) ?? new ConversionWorkingCategory();
            c.Name = model.Name;
            c.ParentId = model.ParentId;
            _workingCategory.InsertOrUpdate(c);
            return IframeScript;
        }

        public ActionResult WorkingCategoryDelete(int id)
        {
            _workingCategory.Delete(id);
            return RedirectToAction("WorkingCategory");
        }
        #endregion
    }
}
