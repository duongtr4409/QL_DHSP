using Com.Gosol.QLKH.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.API.Authorization
{
    public static class PermissionLevel
    {
        public const string READ = "Read";

        public const string CREATE = "Create";

        public const string EDIT = "Edit";

        public const string DELETE = "Delete";

        public const string FULLACCESS = "FullAccess";
    }
    public class CustomAuthAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public ChucNangEnum ChucNangID { get; set; }
        public AccessLevel Quyen { get; set; }
        List<ChucNangEx> dsChucNang { get; set; }
        DieuKienEnum dieuKien { get; set; }

        public CustomAuthAttribute() { }
        public CustomAuthAttribute(ChucNangEnum chucNangID, AccessLevel quyen)
        {
            ChucNangID = chucNangID;
            Quyen = quyen;
        }
        public CustomAuthAttribute(List<ChucNangEx> dsChucNang, DieuKienEnum dieuKien)
        {
            this.dsChucNang = dsChucNang;
            this.dieuKien = dieuKien;
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (CheckAccessLevelByClaims(filterContext))
            {

            }
            else
            {
                filterContext.Result =
                  new ObjectResult(new { Status = -98, Message = "Người dùng không có quyền sử dụng chức năng này." });
            }
        }

        public bool CheckAccessLevelByClaims(AuthorizationFilterContext filterContext)
        {
            try
            {
                var user = filterContext.HttpContext.User as ClaimsPrincipal;
                string type = "";
                //var a = user.Claims.GetType();
                string value = "," + (int)ChucNangID + ",";
                //var a1 = user.Claims.Where(c => c.Type.Equals(type));
                switch (Quyen)
                {
                    case AccessLevel.Read:
                        type = PermissionLevel.READ;
                        break;
                    case AccessLevel.Create:
                        type = PermissionLevel.CREATE;
                        break;
                    case AccessLevel.Edit:
                        type = PermissionLevel.EDIT;
                        break;
                    case AccessLevel.Delete:
                        type = PermissionLevel.DELETE;
                        break;
                    case AccessLevel.FullAccess:
                        type = PermissionLevel.FULLACCESS;
                        break;
                }

                if (user.Claims.Where(c => c.Type.Equals(type) || c.Type.Equals(PermissionLevel.FULLACCESS)).Any(x => x.Value.Contains(value)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CheckAccessLevelByClaims_Ex(AuthorizationFilterContext filterContext)
        {
            try
            {
                var user = filterContext.HttpContext.User as ClaimsPrincipal;
                if (dsChucNang != null && dsChucNang.Count > 0)
                {
                    var resultOr = false;
                    var resultAnd = true;
                    foreach (var item in dsChucNang)
                    {
                        string type = "";
                        string value = "," + item.ChucNang.GetHashCode() + ",";
                        switch (item.AccLevel)
                        {
                            case AccessLevel.Read:
                                type = PermissionLevel.READ;
                                break;
                            case AccessLevel.Create:
                                type = PermissionLevel.CREATE;
                                break;
                            case AccessLevel.Edit:
                                type = PermissionLevel.EDIT;
                                break;
                            case AccessLevel.Delete:
                                type = PermissionLevel.DELETE;
                                break;
                            case AccessLevel.FullAccess:
                                type = PermissionLevel.FULLACCESS;
                                break;
                        }
                        if (user.Claims.Where(c => c.Type.Equals(type) || c.Type.Equals(PermissionLevel.FULLACCESS)).Any(x => x.Value.Contains(value)))
                        {
                            resultOr = true;
                        }
                        else
                        {
                            resultAnd = false;
                        }
                    }
                    if (this.dieuKien == DieuKienEnum.or)
                        return resultOr;
                    return resultAnd;
                }
                else
                {
                    string type = "";
                    string value = "," + (int)ChucNangID + ",";
                    switch (Quyen)
                    {
                        case AccessLevel.Read:
                            type = PermissionLevel.READ;
                            break;
                        case AccessLevel.Create:
                            type = PermissionLevel.CREATE;
                            break;
                        case AccessLevel.Edit:
                            type = PermissionLevel.EDIT;
                            break;
                        case AccessLevel.Delete:
                            type = PermissionLevel.DELETE;
                            break;
                        case AccessLevel.FullAccess:
                            type = PermissionLevel.FULLACCESS;
                            break;
                    }
                    if (user.Claims.Where(c => c.Type.Equals(type) || c.Type.Equals(PermissionLevel.FULLACCESS)).Any(x => x.Value.Contains(value)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var isAuth = await Task.Run(() => CheckAccessLevelByClaims(context));
            if (!isAuth)
            {
                context.Result = new ObjectResult(new { Status = -98, Message = "Người dùng không có quyền sử dụng chức năng này." });
            }

        }

    }
}
