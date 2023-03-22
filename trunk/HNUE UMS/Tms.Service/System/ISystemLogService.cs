using Ums.Core.Domain.System;

namespace Ums.Services.System
{
    public interface ISystemLogService
    {
        void Log(string desc, object item = null);
        void LogAudit(string log);
    }
}
