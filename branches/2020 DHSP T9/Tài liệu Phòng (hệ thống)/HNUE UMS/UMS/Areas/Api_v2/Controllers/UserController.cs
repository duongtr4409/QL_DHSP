using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Ums.App;
using Ums.Core.Entities.Shared;
using Ums.Services.Users;

namespace Ums.Website.Areas.Api_v2.Controllers
{
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IUserGroupService _groupService;
        private readonly IUserTypeService _typeService;
        public UserController()
        {
            _userService = UnityConfig.Resolve<IUserService>();
            _groupService = UnityConfig.Resolve<IUserGroupService>();
            _typeService = UnityConfig.Resolve<IUserTypeService>();
        }

        public JsonResult<ApiResponse> GetGroups()
        {
            var groups = _groupService.Gets().ToList();
            return Json(groups.CreateResponse());
        }

        public JsonResult<ApiResponse> GetTypes()
        {
            var types = _typeService.Gets().ToList();
            return Json(types.CreateResponse());
        }
    }
}
