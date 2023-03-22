using System.Linq;
using Ums.Core.Domain.System;
using Ums.Services.Base;

namespace Ums.Services.System
{
    public interface IFunctionService : IService<Function>
    {
        IQueryable<Function> Gets(int categoryId = 0);
    }
}