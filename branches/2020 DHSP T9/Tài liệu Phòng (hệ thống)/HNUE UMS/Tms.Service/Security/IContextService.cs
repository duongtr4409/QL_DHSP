using Ums.Core.Domain.System;

namespace Ums.Services.Security
{
    public interface IContextService
    {
        StaffUser GetCurrentUser();
        T Resolve<T>() where T : class;
    }

    public interface IApplicationContext
    {
        SystemApplication GetCurrentApplication();
    }
}