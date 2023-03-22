using Hnue.Helper;
using Ums.Services.System;

namespace Ums.Services.Security
{
    public class BaseAuthService : IAuthenticationService
    {
        private readonly IStaffUserService _userService;

        public BaseAuthService(IStaffUserService userService)
        {
            _userService = userService;
        }

        public bool Validate(string username, string password)
        {
            var u = _userService.Get(username);
            return u != null && u.Password == Common.Md5(password);
        }
    }
}