using System.Linq;
using System.Web.Mvc;
using Hnue.Helper;
using Mvc.JQuery.DataTables.DynamicLinq;
using Ums.App.Base;
using Ums.App.Security;
using Ums.Core.Domain.System;
using Ums.Models.Common;
using Ums.Models.System;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Students;
using Ums.Services.System;
using Ums.Services.Users;
using Ums.Models.User;
using OfficeOpenXml;
using System.Collections.Generic;
using Ums.Core.Domain.Users;

namespace Ums.Website.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IStandardStudentService _standardStudentService;
        private readonly IUserGroupService _userGroupService;
        private readonly IUserTypeService _userTypeService;
        private readonly IDepartmentService _departmentService;
        private readonly IStaffUserService _staffUserService;
        private readonly IStaffService _staffService;
        public UserController(IUserService userService, IStandardStudentService standardStudentService, IUserGroupService userGroupService, IUserTypeService userTypeService, IDepartmentService departmentService, IStaffUserService staffUserService, IStaffService staffService)
        {
            _userService = userService;
            _standardStudentService = standardStudentService;
            _userGroupService = userGroupService;
            _userTypeService = userTypeService;
            _departmentService = departmentService;
            _staffUserService = staffUserService;
            _staffService = staffService;
        }

        public ActionResult Import(string file)
        {
            ViewBag.File = file;
            return View();
        }

        [Function("USER_IMPORT")]
        public object GetImport(TableModel model, string file)
        {
            var lst = ReadImport(file);
            return lst.AsQueryable().ToTableResult(lst.Count, model.Draw).ToJson();
        }

        public void StartImport(string file)
        {
            var data = ReadImport(file);
            foreach (var d in data)
            {
                var u = new User();
                d.CopyTo(u);
                u.UserKey = "IMPORT-" + d.Id;
                u.Password = Common.Md5(u.Password);
                if (_userService.Gets().Any(i => i.Username == u.Username || i.Email == u.Email))
                {
                    continue;
                }
                _userService.Insert(u);
            }
        }

        private List<ImportModel> ReadImport(string file)
        {
            var excel = new ExcelPackage(System.IO.File.OpenRead(Server.MapPath(file)));
            var sheet = excel.Workbook.Worksheets.First();
            var lst = new List<ImportModel>();
            for (int i = 2; i <= sheet.Dimension.End.Row; i++)
            {
                var row = new ImportModel
                {
                    Id = sheet.Cells[i, 1].Value.ToString(),
                    UserType = sheet.Cells[i, 2].Value.ToString(),
                    Username = sheet.Cells[i, 3].Value.ToString(),
                    Email = sheet.Cells[i, 4].Value.ToString(),
                    Password = sheet.Cells[i, 5].Value.ToString(),
                    Name = sheet.Cells[i, 6].Value.ToString(),
                    Birthday = sheet.Cells[i, 7].Value.ToString(),
                    Gender = sheet.Cells[i, 8].Value.ToString(),
                    Phone = sheet.Cells[i, 9].Value.ToString(),
                    Address = sheet.Cells[i, 10].Value.ToString()
                };
                lst.Add(row);
            }
            return lst;
        }

        public ActionResult Type(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        [Function("USER_TYPE")]
        public object GetType(TableModel model)
        {
            var lst = _userTypeService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("USER_TYPE")]
        public ActionResult TypeEdit(int id = 0)
        {
            var m = new UserType();
            if (id == 0) return View(m);
            var d = _userTypeService.Get(id);
            d.CopyTo(m);
            return View(m);
        }

        [Function("USER_TYPE")]
        [HttpPost]
        public ActionResult TypeEdit(UserType model)
        {
            var u = _userTypeService.Get(model.Id) ?? new UserType();
            model.CopyTo(u);
            _userTypeService.InsertOrUpdate(u);
            return IframeScript;
        }

        [Function("USER_TYPE")]
        public void TypeDelete(int id)
        {
            _userTypeService.Get(id);
        }

        public ActionResult Group(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        [Function("USER_GROUP")]
        public object GetGroup(TableModel model)
        {
            var lst = _userGroupService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("USER_GROUP")]
        public ActionResult GroupEdit(int id = 0)
        {
            var m = new UserGroup();
            if (id == 0) return View(m);
            var d = _userGroupService.Get(id);
            d.CopyTo(m);
            return View(m);
        }

        [Function("USER_GROUP")]
        [HttpPost]
        public ActionResult GroupEdit(UserGroup model)
        {
            var u = _userGroupService.Get(model.Id) ?? new UserGroup();
            model.CopyTo(u);
            _userGroupService.InsertOrUpdate(u);
            return IframeScript;
        }

        [Function("USER_GROUP")]
        public void GroupDelete(int id)
        {
            var userGroup = _userGroupService.Get(id);
            _userGroupService.Delete(userGroup);
        }

        public ActionResult Id(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        [Function("USER_ID")]
        public object GetUsers(TableModel model)
        {
            var lst = _userService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("USER_ID")]
        public void IdDelete(int id)
        {
            var u = _userService.Get(id);
            _userService.Delete(u);
        }

        public ActionResult Student()
        {
            return View();
        }

        [Function("USER_STUDENT")]
        public object GetStudents(TableModel model)
        {
            return _standardStudentService.GetStudents(model.Start, model.Pagesize, model.Draw).ToJson();
        }

        public ActionResult Password(string keyword = "")
        {
            ViewBag.Keyword = keyword;
            return View();
        }

        [Function("USER_PASSWORD")]
        public object GetPasswords(TableModel model)
        {
            var lst = _userService.Gets();
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.Id,
                    i.Username,
                    i.Name,
                    i.UserType,
                    i.UserKey,
                    i.Email,
                    Session = i.Sessions.Count,
                    LastAccess = i.Sessions.OrderByDescending(j => j.LastAccess).FirstOrDefault()?.LastAccess.ToString() ?? "",
                    Logon = i.Sessions.OrderByDescending(j => j.LastAccess).FirstOrDefault()?.Created.ToString() ?? ""
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }


        #region ACCOUNT
        public ActionResult Staff(int departmentId = 0, string keyword = "")
        {
            ViewBag.Departments = _departmentService.Gets().OrderBy(i => i.TypeId).ThenBy(i => i.Name);
            ViewBag.DepartmentId = departmentId;
            ViewBag.Keyword = keyword;
            return View();
        }

        [Function("USER_STAFF")]
        public object GetStaff(TableModel model, int departmentId, string keyword = "")
        {
            var lst = _staffUserService.Gets(departmentId, keyword)
                .Select(i => new
                {
                    i.Username,
                    i.Email,
                    i.Staff.Name,
                    i.UsersRoles,
                    StaffPositions = i.Staff.Positions,
                    i.IsAdmin,
                    StaffId = i.Staff.Id,
                    UserId = i.Id
                });
            lst = lst.OrderBy(model.Order);
            return lst.Skip(model.Start).Take(model.Pagesize).ToList()
                .Select(i => new
                {
                    i.StaffId,
                    i.Username,
                    i.Email,
                    i.Name,
                    i.UserId,
                    Ps = i.StaffPositions.Aggregate("<ol>", (c, j) => c + "<li>" + j.Position.Name + "</li>") + "</ol>",
                    Rs = i.IsAdmin ? "Admin" : i.UsersRoles.Aggregate("<ol>", (c, j) => c + "<li>" + j.Role?.Name + "</li>") + "</ol>"
                }).AsQueryable().ToTableResult(lst.Count(), model.Draw).ToJson();
        }

        [Function("USER_STAFF")]
        public ActionResult StaffEdit(int id)
        {
            var u = _staffUserService.Get(id);
            var m = new StaffUserModel
            {
                Id = id,
                Name = u.Staff.Name,
                Department = u.Staff.Department.Name,
                IsAdmin = u.IsAdmin,
                Email = u.Email,
                StaffId = u.StaffId,
                Username = u.Username
            };
            ViewBag.DeId = u.Staff.DepartmentId;
            return View(m);
        }

        [Function("USER_STAFF")]
        [HttpPost]
        public ActionResult StaffEdit(StaffUserModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var u = _staffUserService.Get(model.Id);
            u.Email = model.Email;
            u.Username = model.Username;
            u.IsAdmin = model.IsAdmin;
            _staffUserService.Update(u);
            return IframeScript;
        }

        [Function("USER_STAFF")]
        [HttpPost]
        public JsonResult StaffGenerate()
        {
            _staffUserService.Generate(_staffService.Gets());
            return Json(new AjaxResponse());
        }

        [Function("USER_STAFF")]
        [HttpPost]
        public JsonResult StaffResetAll()
        {
            _staffUserService.ResetAll(_staffService.Gets());
            return Json(new AjaxResponse());
        }

        [Function("USER_STAFF")]
        [HttpPost]
        public JsonResult StaffReset(int staffId)
        {
            var user = _staffUserService.Get(staffId);      // dữ liệu truyền lên đang ko phải là StaffId mà là UserKey

            _staffUserService.ResetPassword(_staffService.Get(user.StaffId));
            return Json(new AjaxResponse());
        }

        [Function("USER_STAFF")]
        public ActionResult StaffDelete(int id)
        {
            var u = _staffUserService.Get(id);
            _staffUserService.Delete(u);
            return RedirectToAction("Account", new { deid = u.Staff.DepartmentId });
        }
        #endregion

    }
}