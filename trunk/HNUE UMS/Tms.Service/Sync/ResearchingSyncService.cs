using System;
using System.Collections.Generic;
using System.Net.Http;
using Hnue.Helper;
using Ums.Models.Sync;
using Ums.Services.System;

namespace Ums.Services.Sync
{
    public class ResearchingSyncService : IResearchingSyncService
    {
        private readonly ISettingService _settingService;
        public ResearchingSyncService(ISettingService settingService)
        {
            _settingService = settingService;
        }

        public List<ResearchingSyncTask> Gets(int yearId = 0, int departmentId = 0)
        {
            var client = new HttpClient();
            var uri = new Uri(_settingService.GetValue("SYNC_RESEARCHING").Replace("{{departmentId}}", departmentId.ToString()));
            var str = client.GetStringAsync(uri).GetAwaiter().GetResult();
            var data = str.CastJson<ResearchingSync>();
            return data.Data;
        }
    }
}