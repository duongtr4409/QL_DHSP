using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TEMIS.CMS.Common;
using TEMIS.CMS.Models;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using CoreAPI.Entity;

namespace TEMIS.CMS.Controllers
{
    public class AccountController : Controller
    {

        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Model.TEMIS_systemEntities db = new Model.TEMIS_systemEntities();
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var role = _unitOfWork.GetRepositoryInstance<UserRoles>().GetFirstOrDefaultByParameter(x => x.UserName.Equals(model.UserName) || x.Email.Equals(model.UserName));
            
            //ExceptionLogging.saveLog("role" + role.Role);
            if (role != null)
            {
                if (role.Role.Equals(PublicConstant.ROLE_NCS))
                {
                    ExceptionLogging.saveLog("Pass role");
                    var passwordhash = Utility.Encrypt(model.Password, true);
                    //var checkUser = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => (x.UserName.Equals(model.UserName) || x.Email.Equals(model.UserName)) && x.PassWord == passwordhash);
                    var checkUser = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => (x.UserName.Equals(model.UserName) || x.Email.Equals(model.UserName)));
                    if (checkUser != null)
                    {
                        ExceptionLogging.saveLog("Pass ncs");
                        TaiKhoan loginInfo = new TaiKhoan();
                        loginInfo.Id = checkUser.Id;
                        loginInfo.Username = !string.IsNullOrEmpty(checkUser.UserName) ? checkUser.UserName : checkUser.Email;
                        var ncs = _unitOfWork.GetRepositoryInstance<NCS>().GetFirstOrDefaultByParameter(x => x.Ma == model.UserName || x.Email == model.UserName);
                        if (ncs != null)
                        {
                            loginInfo.Name = ncs.HoTen;
                            loginInfo.Email = ncs.Email;
                        }
                        else
                        {
                            loginInfo.Name = !string.IsNullOrEmpty(checkUser.UserName) ? checkUser.UserName : checkUser.Email;
                            loginInfo.Email = checkUser.Email;
                        }

                        loginInfo.Id = checkUser.Id;


                        Session[PublicConstant.LOGIN_INFO] = loginInfo;
                        ExceptionLogging.saveLog("role =" + role.UserName + role.Role);

                        Session[PublicConstant.ROLE_INFO] = role;
                        ExceptionLogging.saveLog("get role =" + Session[PublicConstant.ROLE_INFO]);
                        if (string.IsNullOrEmpty(checkUser.UserName))
                        {
                            return Redirect("~/Home/Index");
                        }
                        else
                        {
                            return Redirect("~/HoSo/Index");
                        }
                    }
                    else
                    {
                        TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác";
                        return View();
                    }
                }
                else
                {
                    //var loginInfo = await CoreAPI.CoreAPI.Login();
                    //if (loginInfo != null)
                    //{
                    //    returnUrl = loginInfo;
                    //    return Redirect(returnUrl);
                    //}
                    //else
                    //{
                    //    TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác";
                    //    return View();
                    //}

                    TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác";
                    return View();
                }
            }
            else
            {
                //return RedirectToLocal(returnUrl);
                TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác";
                return View();
            }
        }
        //
        // POST: /Account/Login
        public async Task<ActionResult> LoginAdmin(string returnUrl)
        {

            var loginInfo = await CoreAPI.CoreAPI.Login();
            if (loginInfo != null)
            {
                returnUrl = loginInfo;
                return Redirect(returnUrl);
            }
            else
            {
                //return View(model);
                TempData["error"] = "Tài khoản hoặc mật khẩu không chính xác";
                return RedirectToAction("Login", "Account");
            }
        }
        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session[PublicConstant.ROLE_INFO] = null;
            Session[PublicConstant.LOGIN_INFO] = null;
            return RedirectToAction("Login", "Account");
        }


        [AllowAnonymous]
        public ActionResult ChangePass()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }
        public JsonResult UpdatePassword(string OldPassword, string NewPassword, string NewPassword2)
        {
            try
            {
                var user = (TaiKhoan)Session[PublicConstant.LOGIN_INFO];
                var passwordhash = Utility.Encrypt(OldPassword, true);
                var checkUser = _unitOfWork.GetRepositoryInstance<User>().GetFirstOrDefaultByParameter(x => (x.UserName.Equals(user.Username) || x.Email.Equals(user.Username)) && x.PassWord == passwordhash);
                if (checkUser != null)
                {
                    if (NewPassword == NewPassword2)
                    {
                        if (NewPassword.Length > 5)
                        {
                            checkUser.PassWord = Utility.Encrypt(NewPassword, true);
                            checkUser.UpdatedAt = DateTime.Now;
                            checkUser.UpdatedBy = user.Username;
                            _unitOfWork.GetRepositoryInstance<User>().Update(checkUser);
                            _unitOfWork.SaveChanges();
                            return Json("OK", JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json("Mật khẩu mới phải ít nhất 6 kí tự!", JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json("Nhập lại mật khẩu không khớp!", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Sai mật khẩu!", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json("Đổi mật khẩu lỗi ", JsonRequestBehavior.AllowGet);
            }
        }


        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}