using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Security;
using Ums.Core.Domain.System;
using Ums.Models.Common;
using Ums.Models.System;
using Ums.Services.Data;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.System;

namespace Ums.Website.Controllers
{
    [Admin]
    public class SystemController : BaseController
    {
        private readonly IFunctionService _functionService;
        private readonly ICategoryService _systemCategoryService;
        private readonly IRoleService _roleService;
        private readonly IRoleFunctionService _roleFunctionService;
        private readonly IRoleGradeService _roleTrainingSystemService;
        private readonly IGradeService _gradeService;
        private readonly IDepartmentService _departmentService;
        private readonly IStaffUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly ISettingService _settingService;
        private readonly IStaffService _staffService;
        private readonly IApplicationService _applicationService;
        public SystemController(IFunctionService functionService, ICategoryService systemCategoryService, IRoleService roleService, IRoleFunctionService roleFunctionService, IRoleGradeService roleTrainingSystemService, IGradeService gradeService, IDepartmentService departmentService, IStaffUserService userService, IUserRoleService userRoleService, ISettingService settingService, IStaffService staffService, IApplicationService applicationService)
        {
            _functionService = functionService;
            _systemCategoryService = systemCategoryService;
            _roleService = roleService;
            _roleFunctionService = roleFunctionService;
            _roleTrainingSystemService = roleTrainingSystemService;
            _gradeService = gradeService;
            _departmentService = departmentService;
            _userService = userService;
            _userRoleService = userRoleService;
            _settingService = settingService;
            _staffService = staffService;
            _applicationService = applicationService;
        }

        #region SETTING
        public ActionResult Setting()
        {
            return View();
        }

        public object GetSetting(TableModel model)
        {
            var lst = _settingService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult SettingEdit(int id = 0)
        {
            var m = new SettingModel();
            if (id == 0) return View(m);
            var d = _settingService.Get(id);
            m.Key = d.Key;
            m.Value = d.Value;
            m.Desc = d.Desc;
            return View(m);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SettingEdit(SettingModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var d = new SystemSetting
            {
                Key = model.Key,
                Value = model.Value,
                Id = model.Id,
                Desc = model.Desc
            };
            _settingService.InsertOrUpdate(d);
            WorkContext.ClearCache();
            return IframeScript;
        }

        public ActionResult SettingDelete(int id)
        {
            var d = _settingService.Get(id);
            _settingService.Delete(d);
            return RedirectToAction("Setting");
        }

        public ActionResult ClearCache()
        {
            WorkContext.ClearCache();
            return RedirectToAction("Setting");
        }

        public ActionResult Recycle()
        {
            HttpRuntime.UnloadAppDomain();
            return RedirectToAction("Setting");
        }
        #endregion

        #region ROLE
        public ActionResult Role()
        {
            return View();
        }

        public object GetRole(TableModel model)
        {
            var lst = _roleService.Gets().OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Name,
                    Functions = i.RolesFunctions.Aggregate("<ol>", (c, j) => c + "<li>" + j.Function.Name + "</li>") + "</ol>",
                    RoleTs = i.RolesGrades.Aggregate("<ol>", (c, j) => c + "<li>" + j.Grade.Name + "</li>") + "</ol>"
                })
                .AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult RoleEdit(int id = 0)
        {
            var m = new RoleModel();
            if (id == 0) return View(m);
            var f = _roleService.Get(id);
            m.Name = f.Name;
            m.Id = f.Id;
            return View(m);
        }

        [HttpPost]
        public ActionResult RoleEdit(RoleModel m)
        {
            if (!ModelState.IsValid) return View(m);
            var f = new Role
            {
                Name = m.Name,
                Id = m.Id
            };
            _roleService.InsertOrUpdate(f);
            return IframeScript;
        }

        public ActionResult RoleDelete(int id)
        {
            _roleService.Delete(id);
            return RedirectToAction("Role");
        }

        public ActionResult RoleGrade(int id)
        {
            var role = _roleService.Get(id);
            var model = new ListModel
            {
                EntityId = id,
                EntityName = role.Name,
                Selecteds = role.RolesGrades.Select(i => i.GradeId).ToArray(),
                List = _gradeService.Gets().OrderBy(i => i.Name)
                    .Select(i => new SelectListItem { Text = i.Name, Value = i.Id.ToString() })
                    .ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult RoleGrade(ListModel model)
        {
            if (!ModelState.IsValid) return (View(model));
            var rfs = _roleTrainingSystemService.Gets().Where(i => i.RoleId == model.EntityId).ToList();
            foreach (var rf in rfs)
            {
                _roleTrainingSystemService.Delete(rf);
            }
            var lst = model.Selecteds.Select(i => new RoleGrade
            {
                RoleId = model.EntityId,
                GradeId = i
            }).ToList();
            _roleTrainingSystemService.InsertRange(lst);
            return IframeScript;
        }

        public ActionResult RoleFunction(int id)
        {
            var role = _roleService.Get(id);
            var model = new RoleFuncModel
            {
                RoleId = id,
                RoleName = role.Name,
                Selecteds = role.RolesFunctions.Where(i => !i.IsDeleted).Select(i => i.FunctionId).ToArray(),
                List = _systemCategoryService.Gets().OrderBy(i => i.Name).ToList()
                .Select(j => new FuncGroup
                {
                    GroupName = j.Name,
                    List = _functionService.Gets().Where(i => i.CategoryId == j.Id).OrderBy(i => i.Order)
                        .Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        }).ToList()
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult RoleFunction(RoleFuncModel model)
        {
            if (!ModelState.IsValid) return (View(model));
            var rfs = _roleFunctionService.Gets().Where(i => i.RoleId == model.RoleId).ToList();
            foreach (var rf in rfs)
            {
                _roleFunctionService.Delete(rf);
            }
            var lst = model.Selecteds.Select(i => new RoleFunction
            {
                RoleId = model.RoleId,
                FunctionId = i
            }).ToList();
            _roleFunctionService.InsertRange(lst);
            return IframeScript;
        }

        public ActionResult RoleUser(int id)
        {
            ViewBag.Enrolleds = _userRoleService.Gets().Where(i => i.RoleId == id).ToList();
            ViewBag.RoleId = id;
            ViewBag.Departments = _departmentService.Gets();
            return View();
        }

        public JsonResult GetRoleUser(int roleId, int departmentId, string keyword)
        {
            var data = _userService.Gets().Where(i => i.UsersRoles.Where(j => !j.IsDeleted).All(j => j.RoleId != roleId) && i.Staff.Name.Contains(keyword) || i.Email == keyword || i.Staff.Code == keyword);
            if (departmentId > 0)
            {
                data = data.Where(i => i.Staff.DepartmentId == departmentId);
            }
            if (departmentId > 0)
            {
                data = data.Where(i => i.Staff.DepartmentId == departmentId);
            }
            return Json(data.Select(i => new
            {
                i.Id,
                i.Staff.Name,
                i.Email
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public void AddRoleUser(int roleId, int userId)
        {
            if (_userRoleService.Gets().Any(i => i.UserId == userId && i.RoleId == roleId)) return;
            _userRoleService.Insert(new UserRole
            {
                UserId = userId,
                RoleId = roleId
            });
        }

        public void AddRoleUsers(int roleId, string userIds)
        {
            foreach (var userId in userIds.GetInts('-'))
            {
                AddRoleUser(roleId, userId);
            }
        }

        public int RemoveRoleUser(int userRoleId)
        {
            _userRoleService.Delete(_userRoleService.Get(userRoleId));
            return userRoleId;
        }
        #endregion

        #region FUNCTION
        public ActionResult Function(int categoryId = 0)
        {
            ViewBag.Categories = _systemCategoryService.Gets().ToList();
            ViewBag.CategoryId = categoryId;
            return View();
        }

        public object GetFunction(TableModel model, int categoryId = 0)
        {
            var lst = _functionService.Gets(categoryId).OrderBy(i => i.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult FunctionEdit(int id = 0)
        {
            var m = new FunctionModel();
            ViewBag.Categories = _systemCategoryService.Gets().ToList();
            if (id == 0) return View(m);
            var f = _functionService.Get(id);
            f.CopyTo(m);
            return View(m);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FunctionEdit(FunctionModel m)
        {
            if (!ModelState.IsValid) return View(m);
            var f = new Function();
            m.CopyTo(f);
            _functionService.InsertOrUpdate(f);
            return IframeScript;
        }

        public ActionResult FunctionSort(int[] ids)
        {
            var index = 1;
            foreach (var p in ids.Select(i => _functionService.Get(i)))
            {
                p.Order = index++;
                _functionService.Update(p);
            }
            return Json(ids);
        }

        public ActionResult FunctionDelete(int id)
        {
            var f = _functionService.Get(id);
            _functionService.Delete(f);
            return RedirectToAction("Function", new { categoryId = f.CategoryId });
        }
        #endregion

        #region CATEGORY
        public ActionResult Category()
        {
            return View();
        }

        public object GetCategories(TableModel model)
        {
            var lst = _systemCategoryService.Gets().OrderBy(i => i.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult CategoryEdit(int id = 0)
        {
            var m = new FunctionCategoryModel
            {
                Id = id
            };
            if (id == 0) return View(m);
            var f = _systemCategoryService.Get(id);
            m.Name = f.Name;
            m.Icon = f.Icon;
            m.Id = f.Id;
            m.Order = f.Order;
            return View(m);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CategoryEdit(FunctionCategoryModel m)
        {
            if (!ModelState.IsValid) return View(m);
            var f = new SystemCategory
            {
                Name = m.Name,
                Id = m.Id,
                Icon = m.Icon,
                Order = m.Order
            };
            _systemCategoryService.InsertOrUpdate(f);
            return IframeScript;
        }

        public ActionResult CategorySort(int[] ids)
        {
            var index = 1;
            foreach (var p in ids.Select(i => _systemCategoryService.Get(i)))
            {
                p.Order = index++;
                _systemCategoryService.Update(p);
            }
            return Json(ids);
        }

        public ActionResult CategoryDelete(int id)
        {
            _systemCategoryService.Delete(id);
            return RedirectToAction("Category");
        }
        #endregion

        #region LOGS
        public ActionResult Log()
        {
            var path = Server.MapPath("~/logs/system/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var files = Directory.GetFiles(path);
            ViewBag.Files = files.Select(i => i.Substring(i.LastIndexOf('\\') + 1)).OrderByDescending(i => i).ToArray();
            return View();
        }

        public string GetLog(string path)
        {
            var lines = System.IO.File.ReadAllLines(Server.MapPath("~/logs/system/" + path));
            return "[" + lines.JoinArray(',') + "]";
        }

        public ActionResult AuditLog()
        {
            var path = Server.MapPath("~/logs/audit/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var files = Directory.GetFiles(path);
            ViewBag.Files = files.Select(i => i.Substring(i.LastIndexOf('\\') + 1)).OrderByDescending(i => i).ToArray();
            return View();
        }

        public string GetAudit(string path)
        {
            return System.IO.File.ReadAllText(Server.MapPath("~/logs/audit/" + path));
        }
        #endregion

        #region APPLICATION
        public ActionResult Application()
        {
            return View();
        }

        [Function("SYSTEM_APPLICATION")]
        public object GetApplication(TableModel model)
        {
            var lst = _applicationService.Gets().OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        public ActionResult ApplicationEdit(int id = 0)
        {
            var m = new SystemApplication
            {
                Id = id,
                Token = Common.RandomString(128) + Common.GetTimeString()
            };
            if (id == 0) return View(m);
            var f = _applicationService.Get(id);
            return View(f);
        }

        [Function("SYSTEM_APPLICATION")]
        [HttpPost]
        public ActionResult ApplicationEdit(SystemApplication m)
        {
            _applicationService.InsertOrUpdate(m);
            return IframeScript;
        }

        [Function("SYSTEM_APPLICATION")]
        public ActionResult ApplicationDelete(int id)
        {
            _applicationService.Delete(id);
            return RedirectToAction("Application");
        }
        #endregion

        public ActionResult Status()
        {
            return View();
        }

        [Function("SYSTEM_STATUS")]
        public object GetStatus()
        {
            var lst = _settingService.GetDbStat();
            return lst.ToTableResult(lst.Count(), int.MaxValue).ToJson();
        }

    }
}