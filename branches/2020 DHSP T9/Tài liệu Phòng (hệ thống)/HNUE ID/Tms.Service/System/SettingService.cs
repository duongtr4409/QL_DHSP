using System.Data.Entity;
using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.Base;
using Ums.Services.Security;

namespace Ums.Services.System
{
    public class SettingService : Service<SystemSetting>, ISettingService
    {
        public SettingService(DbContext context, IContextService contextService) : base(context, contextService)
        {
        }

        public string GetValue(string key)
        {
            return Get(key)?.Value ?? string.Empty;
        }

        public SystemSetting Get(string key)
        {
            return Gets().FirstOrDefault(i => i.Key == key);
        }

    }
}
