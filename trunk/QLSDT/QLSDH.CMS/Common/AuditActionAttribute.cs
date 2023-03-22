using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CoreAPI.Entity;
using TEMIS.CMS.Repository;
using TEMIS.Model;
using System.Threading;
using System.Threading.Tasks;
namespace TEMIS.CMS.Common
{
    public class AuditActionAttribute : ActionFilterAttribute
    {
        protected DateTime start_time;
        private GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            start_time = DateTime.Now;
            var request = filterContext.HttpContext.Request;

            var controller = filterContext.RequestContext.RouteData.Values["Controller"];
            var action = filterContext.RequestContext.RouteData.Values["Action"];

            RouteData route_data = filterContext.RouteData;
            TimeSpan duration = (DateTime.Now - start_time);
            string controllerName = (string)route_data.Values["controller"];
            string actioName = (string)route_data.Values["action"];
            DateTime created_at = DateTime.Now;
            var loginInfo = (TaiKhoan)HttpContext.Current.Session[PublicConstant.LOGIN_INFO];
            if (loginInfo == null)
            {
                filterContext.Result = new RedirectResult(string.Format("/dang-nhap/"));
            }
            else
            {
                Task.Run(() => {
                    _unitOfWork.GetRepositoryInstance<SysLog>().Add(new SysLog()
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = created_at,
                        Action = actioName,
                        Controller = controllerName,
                        CreatedBy = loginInfo.Username,
                        AreaAccessed = request.RawUrl,
                        IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                    });
                    _unitOfWork.SaveChanges();
                });
            }
        }
    }
}