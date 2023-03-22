using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Hnue.Helper;
using Ums.App.Base;
using Ums.App.Constant;
using Ums.Core.Entities.Security;
using Ums.Core.Entities.Shared;
using Ums.Models.Common;
using Ums.Models.Security;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Security;
using Ums.Services.System;

namespace Ums.Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISignInService _signInService;
        private readonly IStaffUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly IStaffService _staffService;
        private readonly ISettingService _settingService;
        public AccountController(IAuthenticationService authenticationService, ISignInService signInService, IStaffUserService userService, IDepartmentService departmentService, IStaffService staffService, ISettingService settingService)
        {
            _authenticationService = authenticationService;
            _signInService = signInService;
            _userService = userService;
            _departmentService = departmentService;
            _staffService = staffService;
            _settingService = settingService;
        }

        public ActionResult Login(string message = "")
        {
            ViewBag.Message = message;
            return View();
        }

        public ActionResult OAuth()
        {
            var uauthLogin = _settingService.GetValue("HNUE_ID_OAUTH_LOGIN_ENPOINT");
            var host = Request.IsLocal ? "http://" + Request.Url.Host + ":" + Request.Url.Port : "//" + Request.Url.Host;
            var url = uauthLogin.Replace("{{returnUrl}}", host + Url.Action("AuthResponse"));
            return Redirect(url);
        }

        public ActionResult AuthResponse(string accessToken)
        {
            var oauthEnpoint = _settingService.GetValue("HNUE_ID_OAUTH_GET_USER_ENPOINT");
            var appToken = _settingService.GetValue("HNUE_ID_OAUTH_APP_TOKEN");
            var client = new WebClient();
            client.Headers.Add("token", appToken);
            try
            {
                var u = client.DownloadString(oauthEnpoint + accessToken).CastJson<ResultData>().data.CastJson<OAuthUser>();
                _signInService.SignIn(u.Username);
            }
            catch
            {
            }
            return Redirect("/");
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            _signInService.SignOut();
            WorkContext.ClearCache();
            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult Info()
        {
            ViewBag.Departments = _departmentService.Gets()
                .Select(i => new { i.Id, i.Name, i.TypeId }).ToList()
                .OrderBy(i => i.TypeId).ThenBy(i => i.Name)
                .Select(i => new KeyValuePair<string, int>(i.Name, i.Id)).ToList();
            return View();
        }
    }
}