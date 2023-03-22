using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Ums.App;
using Ums.App.Base;
using Ums.Core.Entities.Shared;
using Ums.Services.Security;
using Ums.Services.System;

namespace Ums.Website.Areas.Api_v2.Controllers
{
    public class StaffUserController : BaseApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ISystemLogService _systemLogService;
        private readonly IStaffUserService _staffUserService;
        public StaffUserController()
        {
            _systemLogService = UnityConfig.Resolve<ISystemLogService>();
            _authenticationService = UnityConfig.Resolve<IAuthenticationService>();
            _staffUserService = UnityConfig.Resolve<IStaffUserService>();
        }

        [HttpPost]
        public JsonResult<ApiResponse> Validate(string username, string password)
        {
            var validateService = _authenticationService;
            if (validateService.Validate(username, password))
            {
                var user = _staffUserService.Get(username);
                var r = new { user.Id, user.Email, user.Username, user.Staff.Name, user.StaffId };
                _systemLogService.LogAudit("API validate success for user: " + username);
                return Json(r.CreateResponse());
            }
            _systemLogService.LogAudit("API validate false for user: " + username);
            return Json(this.CreateResponse(false, "Invalid UserName or Password"));
        }

        public JsonResult<ApiResponse> GetUsers(int departmentId, int page = 1, int pagesize = 30)
        {
            var userService = _staffUserService;
            var lst = userService.Gets(departmentId: departmentId);
            var users = lst.OrderBy(i => i.Id).Skip((page - 1) * pagesize).Take(pagesize).Select(i => new
            {
                i.Id,
                i.Username,
                i.Email,
                i.Staff.Name,
                i.StaffId,
                i.Staff.DepartmentId
            }).ToList();
            _systemLogService.LogAudit("API Get all users");
            return Json(users.CreateResponse(total: lst.Count()));
        }

        [HttpPost]
        public JsonResult<ApiResponse> ChangePassword(int userId, string newPassword)
        {
            var userService = _staffUserService;
            userService.ChangePassword(userId, newPassword);
            _systemLogService.LogAudit("API Change password success");
            return Json(true.CreateResponse(_message: "Đã đổi mật khẩu"));
        }
    }
}
