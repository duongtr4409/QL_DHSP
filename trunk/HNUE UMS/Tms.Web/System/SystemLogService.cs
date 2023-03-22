using Hnue.Helper;
using System;
using System.IO;
using System.Web;
using Ums.App.Helper;
using Ums.Core.Domain.System;
using Ums.Services.Security;
using Ums.Services.System;

namespace Ums.App.System
{
    public class SystemLogService : ISystemLogService
    {
        private readonly IContextService _contextService;
        private readonly IApplicationContext _applicationContext;
        public SystemLogService(IContextService contextService, IApplicationContext applicationContext)
        {
            _contextService = contextService;
            _applicationContext = applicationContext;
        }

        public void Log(string desc, object item = null)
        {
            var user = _contextService.GetCurrentUser();
            var obj = new { Desc = desc, Data = item, User = new { user.Username, user.Email, user.Staff.Name } };
            var path = HttpContext.Current.Server.MapPath("~/logs/system/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllLines(path + DateTime.Now.ToString("dd-MM-yyyy") + ".log", new[] { obj.ToJson() });
        }

        public void LogAudit(string log)
        {
            var app = _applicationContext.GetCurrentApplication();
            var line = $"[{app.Name}] {DateTime.Now.ToAppDateTime()}: " + log;
            var path = HttpContext.Current.Server.MapPath("~/logs/audit/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            File.AppendAllLines(path + DateTime.Now.ToString("dd-MM-yyyy") + ".log", new[] { line });
        }
    }
}
