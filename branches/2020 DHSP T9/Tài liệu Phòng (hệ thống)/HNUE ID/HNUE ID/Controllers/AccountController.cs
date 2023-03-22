using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Hnue.Helper;
using Ums.App.Base;
using Ums.App.Provider;
using Ums.Core.Entities.Shared;
using Ums.Models.Common;
using Ums.Models.Security;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Security;
using Ums.Services.System;
using Ums.Services.Users;

namespace Ums.Website.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISignInService _signInService;
        private readonly IStaffUserService _systemUser;
        private readonly IDepartmentService _departmentService;
        private readonly IStaffService _staffService;
        private readonly IUserService _userService;
        private readonly ISettingService _settingService;
        public AccountController(IAuthenticationService authenticationService, ISignInService signInService, IStaffUserService systemUser, IDepartmentService departmentService, IStaffService staffService, IUserService userService, ISettingService settingService)
        {
            _authenticationService = authenticationService;
            _signInService = signInService;
            _systemUser = systemUser;
            _departmentService = departmentService;
            _staffService = staffService;
            _userService = userService;
            _settingService = settingService;
        }

        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        public ActionResult RegisterSuccess(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!Request.IsLocal)
            {
                var httpclient = new HttpClient();
                var parameters = new Dictionary<string, string> { { "secret", SettingProvider.GetValue("GOOGLE_CAPCHA_SECRET_KEY") }, { "response", model.Capcha } };
                var encodedContent = new FormUrlEncodedContent(parameters);
                var response = httpclient.PostAsync(SettingProvider.GetValue("GOOGLE_CAPCHA_VERIFY_URL"), encodedContent).Result;
                var r = response.Content.ReadAsStringAsync().Result.CastJson<TokenResponseModel>();
                if (!r.Success)
                {
                    ModelState.AddModelError("InvalidCapcha", "Sai mã bảo vệ, vui lòng thử lại!");
                    return View(model);
                }
            }
            var result = _userService.Validate(model);
            if (!result.success)
            {
                ModelState.AddModelError("", result.message);
                return View(model);
            }
            _userService.CreateUser(model);
            return RedirectToAction("RegisterSuccess", new { returnUrl });
        }

        public ActionResult Login(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!Request.IsLocal)
            {
                var httpclient = new HttpClient();
                var parameters = new Dictionary<string, string> { { "secret", SettingProvider.GetValue("GOOGLE_CAPCHA_SECRET_KEY") }, { "response", model.Capcha } };
                var encodedContent = new FormUrlEncodedContent(parameters);
                var response = httpclient.PostAsync(SettingProvider.GetValue("GOOGLE_CAPCHA_VERIFY_URL"), encodedContent).Result;
                var result = response.Content.ReadAsStringAsync().Result.CastJson<TokenResponseModel>();
                if (!result.Success)
                {
                    ModelState.AddModelError("InvalidCapcha", "Sai mã bảo vệ, vui lòng thử lại!");
                    return View(model);
                }
            }
            var user = _authenticationService.Validate(model.Username, model.Password, model.Type);
            if (user == null)
            {
                ModelState.AddModelError("InvalidUsernameOrPassword", "Sai tên đăng nhập hoặc mật khẩu. Vui lòng thử lại!");
                return View(model);
            }
            if (_settingService.GetValue("HNUE_ID_ENABLE_2FACTOR_AUTH").ToBool())
            {
                var email = Common.IsValidEmail(user.Username) ? user.Username : Common.IsValidEmail(user.Email) ? user.Email : "";
                if (!string.IsNullOrEmpty(email))
                {
                    var key = _userService.SetTwoFactorAuth(email);
                    Session.Add("TWO_AUTH", new TwoFactorAuth { email = email, userId = user.Id, created = DateTime.Now, key = key });
                    return RedirectToAction("TwoFactorAuth", new { returnUrl });
                }
            }
            _signInService.SignIn(user.Id);
            return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
        }

        public ActionResult TwoFactorAuth(string returnUrl)
        {
            ViewBag.Data = Session["TWO_AUTH"];
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TwoFactorAuth(string returnUrl, FormCollection form)
        {
            var key = form.GetValue("Key")?.AttemptedValue;
            var session = (TwoFactorAuth)Session["TWO_AUTH"];
            if (session == null)
            {
                return RedirectToAction("Login", new { returnUrl });
            }
            if (session.created.AddMinutes(15) < DateTime.Now)
            {
                ModelState.AddModelError("ExpireTwoFactor", "Mã bảo mật đã hết hạn. Vui lòng đăng nhập lại!");
                ViewBag.Data = session;
                return View();
            }
            if (session.key != key)
            {
                ModelState.AddModelError("InvalidTwoFactor", "Sai mã bảo mật. Vui lòng thử lại!");
                ViewBag.Data = session;
                return View();
            }
            _signInService.SignIn(session.userId);
            return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> FacebookLogin(string token)
        {
            var enpoint = _settingService.GetValue("FACEBOOK_LOGIN_VALIDATE");
            var client = new HttpClient();
            var result = await client.GetAsync(enpoint + token).Result.Content.ReadAsStringAsync();
            var model = result.CastJson<FacebookTokenModel>();
            var id = _authenticationService.Validate(model);
            if (id == 0)
            {
                return Json(false.CreateResponse(false, "Đăng nhập không thành công!"));
            }
            _signInService.SignIn(id);
            return Json(model.CreateResponse());
        }

        [HttpPost]
        public async System.Threading.Tasks.Task<JsonResult> GoogleLogin(string token)
        {
            var enpoint = _settingService.GetValue("GOOGLE_OAUTH2_TOKEN_INFO");
            var client = new HttpClient();
            var result = await client.GetAsync(enpoint + token).Result.Content.ReadAsStringAsync();
            var model = result.CastJson<GoogleTokenModel>();
            var id = _authenticationService.Validate(model);
            if (id == 0)
            {
                return Json(false.CreateResponse(false, model.error));
            }
            _signInService.SignIn(id);
            return Json(true.CreateResponse());
        }

        [HttpPost]
        public int InitRecoverPassword(string email)
        {
            if (!Common.IsValidEmail(email)) return 0;
            return _userService.InitRecoverPassword(email, "//" + Request.Url.Host + Url.Action("RecoverPassword"));
        }

        public ActionResult RecoverPassword(string email, string token)
        {
            var pass = _userService.RecoverPassword(email, token);
            if (!string.IsNullOrEmpty(pass))
            {
                return Content($"<h4 style='text-align:center'>Mật khẩu mới của bạn là: <b>{pass}</b>.<br/><a href='/'>Trở lại</a></h4>");
            }
            return Content("<h4 style='text-align:center'>Không khởi tạo được mật khẩu của bạn, vui lòng thử lại sau.</h4>");
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            _signInService.SignOut();
            WorkContext.ClearCache();
            return RedirectToAction("Login");
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