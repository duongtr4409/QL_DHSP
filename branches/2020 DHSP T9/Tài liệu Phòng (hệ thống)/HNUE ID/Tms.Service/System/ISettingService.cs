using Ums.Core.Domain.System;
using Ums.Services.Base;

namespace Ums.Services.System
{
    public interface ISettingService : IService<SystemSetting>
    {
        string GetValue(string key);
        SystemSetting Get(string key);
    }
}