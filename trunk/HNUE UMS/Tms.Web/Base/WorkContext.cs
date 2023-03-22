using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Hnue.Helper;
using Ums.App.Provider;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.System;
using Ums.Services.System;

namespace Ums.App.Base
{
    public class WorkContext
    {
        public static StaffUser UserInfo
        {
            get
            {
                var u = HttpContext.Current.Items[Constant.Security.CurrentUserKey] as StaffUser;
                if (u != null && u.Id > 0) return u;
                u = UnityConfig.Resolve<IStaffUserService>().Get(HttpContext.Current.User.Identity.Name);
                if (u == null)
                {
                    FormsAuthentication.SignOut();
                    HttpContext.Current.Response.Redirect(SettingProvider.GetValue(Constant.Security.LOGIN_URL) + "?message=Bạn không có quyền truy cập! Vui lòng đăng nhập lại!");
                    return null;
                }
                HttpContext.Current.Items[Constant.Security.CurrentUserKey] = u;
                return u;
            }
        }

        public static Staff Staff => UserInfo.Staff;

        public static Department Department => Staff.Department;

        public static List<Role> Roles
        {
            get
            {
                List<Role> rs;
                if (HttpContext.Current.Items[Constant.Security.CHANGE_ROLE] == null)
                {
                    rs = HttpContext.Current.Items[Constant.Security.CurrentRoleKey] as List<Role>;
                    if (rs != null) return rs;
                }
                rs = UserInfo.IsAdmin ? UnityConfig.Resolve<IRoleService>().Gets().ToList() : UnityConfig.Resolve<IRoleService>().GetUserRoles(UserInfo.Id).ToList();
                HttpContext.Current.Items[Constant.Security.CurrentRoleKey] = rs;
                return rs;
            }
        }

        public static List<Function> Functions
        {
            get
            {
                List<Function> fs;
                if (HttpContext.Current.Items[Constant.Security.CHANGE_ROLE] == null)
                {
                    fs = HttpContext.Current.Items[Constant.Security.CurrentFunctionKey] as List<Function>;
                    if (fs != null && fs.Count > 0) return fs;
                }
                if (UserInfo == null) return new List<Function>();
                if (UserInfo.IsAdmin)
                {
                    fs = UnityConfig.Resolve<IFunctionService>().Gets().ToList();
                }
                else
                {
                    fs = UnityConfig.Resolve<IFunctionService>().Gets().Where(i => i.IsPublic).ToList();
                    fs.AddRange(Roles.SelectMany(i => i.RolesFunctions).Select(i => i.Function).OrderBy(i => i.Order).ToList());
                }
                HttpContext.Current.Items[Constant.Security.CurrentFunctionKey] = fs;
                return fs;
            }
        }

        public static string GetMenu()
        {
            var result = HttpContext.Current.Items[Constant.Security.CurrentMainMenu] as string;
            if (!string.IsNullOrEmpty(result)) return result;
            var cats = UnityConfig.Resolve<ICategoryService>().Gets().OrderBy(i => i.Order).ThenBy(i => i.Name).ToList();
            var funcs = AllFunctions.ToList();
            var functions = Functions.Select(i => i.Id).ToArray();
            foreach (var i in cats)
            {
                var children = funcs.Where(j => j.CategoryId == i.Id && functions.Contains(j.Id)).ToList();
                if (children.Count < 1) continue;
                result += "<li>";
                result += $"<a title='{i.Name}' href='javascript:void(0)'>{i.Icon}<span>{i.Name}</span><span class='clear'></span></a>";
                result += "<ul class='children'>";
                result = children.Aggregate(result, (current, c) => current + $"<li><a title='{c.Name}' href='{c.Url}'>{c.Icon}<span>{c.Name}</span></a></li>");
                result += "</ul></li>";
            }
            HttpContext.Current.Items[Constant.Security.CurrentMainMenu] = result;
            return result;
        }

        private static IEnumerable<Function> AllFunctions
        {
            get
            {
                if (HttpContext.Current.Items["AllFunctions"] is List<Function> fs && fs.Count > 0) return fs;
                fs = UnityConfig.Resolve<IFunctionService>().Gets().OrderBy(j => j.Order).ToList();
                HttpContext.Current.Items["AllFunctions"] = fs;
                return fs;
            }
        }

        public static bool IsChangingRole => UserInfo.Username != HttpContext.Current.User.Identity.Name && UserInfo.Email != HttpContext.Current.User.Identity.Name;

        public static string AppVersion => "2.0 beta";

        public static void ClearCache()
        {
            HttpContext.Current.Items.Clear();
            SettingProvider.ClearCache();
        }
    }
}