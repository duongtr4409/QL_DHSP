using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Hnue.Helper;
using Ums.Models.Sync;
using Ums.Services.Conversion;
using Ums.Services.Data;
using Ums.Services.Organize;
using Ums.Services.System;

namespace Ums.Services.Sync
{
    public class TeachingSyncService : ITeachingSyncService
    {
        private readonly ISettingService _settingService;
        private readonly IYearService _yearService;
        private readonly IDepartmentService _departmentService;
        private readonly ITeachingService _teachingConversionService;
        public TeachingSyncService(ISettingService settingService, IYearService yearService, IDepartmentService departmentService, ITeachingService teachingConversionService)
        {
            _settingService = settingService;
            _yearService = yearService;
            _departmentService = departmentService;
            _teachingConversionService = teachingConversionService;
        }

        public List<TeachingSyncTask> GetStandard(int yearId = 0, int departmentId = 0, int semester = 0, int gradeId = 0, int conversionId = 0)
        {
            string token = "";
            using (var client = new WebClient())
            {
                var loginData = new
                {
                    UserName = _settingService.GetValue("SYNC_STANDARD_USERNAME"),
                    UserPassword = _settingService.GetValue("SYNC_STANDARD_PASSWORD"),
                };
                client.Headers.Add("Content-Type", "application/json");
                var login = client.UploadString(_settingService.GetValue("SYNC_STANDARD_LOGIN"), loginData.ToJson());
                token = login.CastJson<List<StandardLogin>>().FirstOrDefault()?.Token;
                if (string.IsNullOrEmpty(token))
                {
                    return new List<TeachingSyncTask>();
                }
            }
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + token);
                client.Headers.Add("Content-Type", "application/json");
                client.Encoding = Encoding.UTF8;
                var year = _yearService.Get(yearId);
                var department = _departmentService.Get(departmentId);
                var conversion = _teachingConversionService.Get(conversionId);
                var requestData = new
                {
                    Hoc_ky = semester,
                    Nam_hoc = year.From.Year + "-" + year.To.Year,
                    Ma_khoa = department.SyncCode,
                    Ma_cb = ""
                };
                var response = client.UploadString(_settingService.GetValue("SYNC_STANDARD_URL"), requestData.ToJson());
                return response.CastJson<List<StandardTask>>()
                    .Select(i => new TeachingSyncTask
                    {
                        YearId = yearId,
                        GradeId = gradeId,
                        ConversionId = conversionId,
                        Conversion = conversion.Name,
                        DepartmentId = departmentId,
                        ForDepartmentId = departmentId,
                        Name = i.Ten_mon,
                        LessionTime = i.So_tiet.ToDouble(),
                        Class = i.Ten_lop + "(" + i.Loai_lop + ")",
                        Size = i.Sy_so.ToInt(),
                        SubjectName = i.Ten_mon,
                        SubjectCode = i.Ky_hieu,
                        Desc = i.ToJson(),
                        Lecturer = i.Ho_ten,
                        SyncId = yearId + i.Ma_khoa + i.Ma_cb + i.Ten_lop + i.Ky_hieu
                    }).ToList();
            }
        }

        public List<TeachingSyncTask> GetOffCampus(int yearId = 0, int departmentId = 0, int gradeId = 0)
        {
            var client = new HttpClient();
            var uri = new Uri(_settingService.GetValue("SYNC_OFF_CAMPUS_URL").Replace("{{yearId}}", yearId.ToString()).Replace("{{departmentId}}", departmentId.ToString()));
            var str = client.GetStringAsync(uri).GetAwaiter().GetResult();
            var data = str.CastJson<List<TeachingSyncTask>>();
            if (gradeId > 0)
            {
                data = data.Where(i => i.GradeId == gradeId).ToList();
            }
            return data;
        }

        public List<TeachingSyncTask> GetPostGraduated(int yearId = 0, int departmentId = 0, int gradeId = 0)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("token", _settingService.GetValue("SYNC_POST_GRADUATED_TOKEN"));
            var uri = new Uri(_settingService.GetValue("SYNC_POST_GRADUATED_URI").Replace("{{yearId}}", yearId.ToString()).Replace("{{departmentId}}", departmentId.ToString()));
            var str = client.GetStringAsync(uri).GetAwaiter().GetResult();
            var data = str.CastJson<PostGraduatedSyncModel>().Result;
            if (gradeId > 0)
            {
                data = data.Where(i => i.GradeId == gradeId).ToList();
            }
            return data;
        }
    }
}