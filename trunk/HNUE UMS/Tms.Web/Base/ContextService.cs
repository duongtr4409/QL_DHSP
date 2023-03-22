using System.Web;
using Ums.Core.Domain.System;
using Ums.Services.Security;

namespace Ums.App.Base
{
    public class ContextService : IContextService
    {
        public StaffUser GetCurrentUser()
        {
            return WorkContext.UserInfo;
        }

        public T Resolve<T>() where T : class
        {
            return UnityConfig.Resolve<T>();
        }
    }
    public class ApiContextController : IApplicationContext
    {
        public SystemApplication GetCurrentApplication()
        {
            return (SystemApplication) HttpContext.Current.Items["CurrentApplication"];
        }
    }
}
