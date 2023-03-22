using System.Collections.Generic;
using Ums.Core.Domain.Data;
using Ums.Services.Base;

namespace Ums.Services.Data
{
    public interface IClassifyService : IService<Classify>
    {
        List<KeyValuePair<string, int>> ClassifiesStatistic(int yearId = 0, int departmentId = 0);
    }
}