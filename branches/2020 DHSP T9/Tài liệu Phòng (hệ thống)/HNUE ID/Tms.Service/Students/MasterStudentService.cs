using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ums.Core.Domain.Students;
using Ums.Services.System;

namespace Ums.Services.Students
{
    public class MasterStudentService : IMasterStudentService
    {
        private readonly ISettingService _settingService;
        public readonly IDbConnection Db;
        public MasterStudentService(ISettingService settingService)
        {
            _settingService = settingService;
            Db = new SqlConnection(_settingService.GetValue("HNUE_MASTER_CONNECTION"));
        }

        public MasterStudent GetStudent(string mahv)
        {
            return Db.QuerySingle<MasterStudent>("SELECT [Id],[MaHv],[Ho],[Ten],[NgaySinh],[Email],[DT],[Code],DiaChi FROM [HNUE_MASTER_ONLINE] where mahv=@mahv or email = @mahv", new { mahv });
        }
    }
}
