using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.System;

namespace Ums.App.Provider
{
    public class SettingProvider
    {
        private static List<SystemSetting> _settings = UnityConfig.Resolve<ISettingService>().Gets().ToList();

        public static string GetValue(string key)
        {
            return _settings.FirstOrDefault(i => i.Key == key)?.Value ?? $"[{key}]";
        }

        public static void ClearCache()
        {
            _settings = UnityConfig.Resolve<ISettingService>().Gets().ToList();
        }
    }
}
