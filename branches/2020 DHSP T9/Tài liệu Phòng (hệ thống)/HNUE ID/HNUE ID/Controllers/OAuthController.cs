using Hnue.Helper;
using System.Web.Mvc;
using Ums.Core.Entities.Shared;
using Ums.Services.OAuth;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Security;
using Ums.Services.Users;
using Ums.Website.Security;

namespace Ums.Website.Controllers
{
    public class OAuthController : Controller
    {
        private readonly IOAuthTokenService _oauthService;
        private readonly ISessionService _sessionService;
        private readonly ISignInService _signInService;
        private readonly IDepartmentService _departmentService;
        private readonly IStaffUserService _staffUserService;
        private readonly IStaffService _staffService;
        public OAuthController(IOAuthTokenService oauthService, ISessionService sessionService, ISignInService signInService, IDepartmentService departmentService, IStaffUserService staffUserService, IStaffService staffService)
        {
            _oauthService = oauthService;
            _sessionService = sessionService;
            _signInService = signInService;
            _departmentService = departmentService;
            _staffUserService = staffUserService;
            _staffService = staffService;
        }

        public ActionResult Access(string returnUrl)
        {
            var session = _sessionService.GetBySessionId(HttpContext.User.Identity.Name);
            if (session == null)
            {
                return Redirect("/");
            }
            var token = _oauthService.CreateAccessToken(session.Id);
            var url = returnUrl.Contains("?") ? returnUrl + "&accessToken=" + token.AccessToken : returnUrl + "?accessToken=" + token.AccessToken;
            return Redirect(url);
        }

        public ActionResult Login(string returnUrl)
        {
            _signInService.SignOut();
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Access", new { returnUrl }) });
        }

        [OAuth]
        public JsonResult GetUser(string accessToken)
        {
            var user = _oauthService.GetUser(accessToken);
            if (user == null)
            {
                return Json(false.CreateResponse(false, "Access denined!"), JsonRequestBehavior.AllowGet);
            }
            var staff = _staffUserService.Get(user.UserKey.ToInt());
            return Json(new
            {
                user.Id,
                user.Name,
                user.Email,
                user.UserType,
                user.Phone,
                user.Username,
                user.UserKey,
                StaffId = user.UserType == "STAFF" ? staff?.StaffId : 0,
                DepartmentId = user.UserType == "STAFF" ? staff?.Staff?.DepartmentId : 0
            }.CreateResponse(), JsonRequestBehavior.AllowGet);
        }
    }
}