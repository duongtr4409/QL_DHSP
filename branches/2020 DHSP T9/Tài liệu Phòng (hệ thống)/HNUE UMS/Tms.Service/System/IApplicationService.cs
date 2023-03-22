using Ums.Core.Domain.System;
using Ums.Services.Base;

namespace Ums.Services.System
{
    public interface IApplicationService : IService<SystemApplication>
    {
        SystemApplication GetByToken(string token);
        bool Validate(string token);
    }
}