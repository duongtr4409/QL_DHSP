using System.Web;
using System.Web.Security;
using Ums.App.Provider;
using Ums.Core.Domain.Users;
using Ums.Services.Users;

namespace Ums.App.Base
{
    public class WorkContext
    {
        public static User UserInfo
        {
            get
            {
                var u = HttpContext.Current.Items[Constant.Security.CurrentUserKey] as User;
                if (u != null && u.Id > 0) return u;
                u = UnityConfig.Resolve<ISessionService>().GetUser(HttpContext.Current.User.Identity.Name);
                if (u == null)
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect(SettingProvider.GetValue(Constant.Security.LOGIN_URL));
                    return null;
                }
                HttpContext.Current.Items[Constant.Security.CurrentUserKey] = u;
                return u;
            }
        }

        public static string AppVersion => "1.0 beta";

        public static void ClearCache()
        {
            HttpContext.Current.Items.Clear();
            SettingProvider.ClearCache();
        }
    }
}