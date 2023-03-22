using System.Collections.Generic;
using Ums.Models.Sync;

namespace Ums.Services.Sync
{
    public interface ITeachingSyncService
    {
        List<TeachingSyncTask> GetStandard(int yearId = 0, int departmentId = 0, int semester = 0, int gradeId = 0, int conversionId = 0);
        List<TeachingSyncTask> GetOffCampus(int yearId = 0, int departmentId = 0, int gradeId = 0);
        List<TeachingSyncTask> GetPostGraduated(int yearId = 0, int departmentId = 0, int gradeId = 0);
    }
}