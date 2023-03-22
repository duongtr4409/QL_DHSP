using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Hnue.Helper;
using Ums.App.Base;
using Ums.App.Helper;
using Ums.App.Provider;
using Ums.Core.Domain.Users;
using Ums.Core.Entities.Shared;
using Ums.Models.Account;
using Ums.Models.Common;
using Ums.Models.Security;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Security;
using Ums.Services.System;
using Ums.Services.Users;

namespace Ums.Website.Controllers
{
    public class UserController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISignInService _signInService;
        private readonly IStaffUserService _systemUser;
        private readonly IDepartmentService _departmentService;
        private readonly IStaffService _staffService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        public UserController(IAuthenticationService authenticationService, ISignInService signInService, IStaffUserService systemUser, IDepartmentService departmentService, IStaffService staffService, IUserService userService, ISettingService settingService)
        {
            _authenticationService = authenticationService;
            _signInService = signInService;
            _systemUser = systemUser;
            _departmentService = departmentService;
            _staffService = staffService;
            _userService = userService;
            _settingService = settingService;
        }

        [Authorize]
        public ActionResult Info()
        {
            return View();
        }

        [Authorize]
        public ActionResult InfoEdit()
        {
            var m = new UserModel();
            WorkContext.UserInfo.CopyTo(m);
            m.Birthday = WorkContext.UserInfo.Birthday?.ToAppDate();
            return View(m);
        }

        [Authorize]
        [HttpPost]
        public ActionResult InfoEdit(UserModel user)
        {
            var u = _userService.Get(user.Id);
            u.Avatar = user.Avatar;
            u.Address = user.Address;
            u.Birthday = user.Birthday.ToAppDate();
            u.Phone = user.Phone;
            u.Name = user.Name;
            _userService.Update(u);
            return RedirectToAction("Info");
        }

        [Authorize]
        public ActionResult Security()
        {
            return View();
        }

        [Authorize]
        public JsonResult GetStaves(int departmentId)
        {
            var d = _staffService.Gets(departmentId)
                .Select(i => new
                {
                    i.Id,
                    Name = i.Name + " [" + i.Title.Name + "] ",
                    StaffPositions = i.Positions
                }).OrderBy(i => i.Name).ToList()
                .Select(i => new
                {
                    i.Id,
                    Name = i.Name + " [" + i.StaffPositions.Aggregate("", (c, j) => c + j.Position.Name + ", ") + "]"
                }).ToArray();
            return Json(d, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public JsonResult SaveEmail(string email)
        {
            email = email.ToLower();
            var regex = new Regex(@"^([\w\.\-]+)@hnue\.edu\.vn$");
            if (!regex.Match(email).Success)
            {
                return Json(AjaxResponse.New(0, message: "Sai định dạng email!"));
            }
            if (_systemUser.Gets().Any(i => i.Email == email && i.Id != WorkContext.UserInfo.Id))
            {
                return Json(AjaxResponse.New(0, message: "Email này đã có người sử dụng!"));
            }
            var u = _systemUser.GetByUsername(WorkContext.UserInfo.Username);
            u.Email = email;
            _systemUser.Update(u);
            return Json(AjaxResponse.New());
        }

        [HttpPost]
        [Authorize]
        public JsonResult ChangePassword(string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(newPassword)) return Json(3);
            if (WorkContext.UserInfo.Password != Common.Md5(oldPassword)) return Json(2);
            _userService.ChangePassword(WorkContext.UserInfo.Id, newPassword);
            return Json(1);
        }
    }
}