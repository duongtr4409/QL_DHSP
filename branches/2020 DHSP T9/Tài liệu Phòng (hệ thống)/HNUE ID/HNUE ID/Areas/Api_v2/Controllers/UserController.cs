using System.Web.Http;
using System.Web.Http.Results;
using Ums.App;
using Ums.Core.Entities.Shared;
using Ums.Services.Security;
using Ums.Services.Users;
using Ums.Website.Security;

namespace Ums.Website.Areas.Api_v2.Controllers
{
    [Api]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        public UserController()
        {
            _userService = UnityConfig.Resolve<IUserService>();
            _authenticationService = UnityConfig.Resolve<IAuthenticationService>();
        }

        [HttpPost]
        public JsonResult<ApiResponse> Login(string username, string password, string type)
        {
            var user = _authenticationService.Validate(username, password, type);
            if (user != null)
            {
                var r = new { user.Id, user.Email, user.Username, user.Name, user.StaffId, user.Address, user.Avatar, user.Birthday, user.Phone };
                return Json(r.CreateResponse());
            }
            return Json(this.CreateResponse(false, "Invalid UserName or Password"));
        }
    }
}
