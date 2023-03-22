using System.Collections.Generic;
using Ums.Models.Sync;

namespace Ums.Services.Sync
{
    public interface IResearchingSyncService
    {
        List<ResearchingSyncTask> Gets(int yearId = 0, int departmentId = 0);
    }
}