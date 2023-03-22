using System.Collections.Generic;
using Ums.Core.Domain.Personnel;
using Ums.Core.Entities.Statistic;

namespace Ums.Services.Statistic
{
    public interface IStatisticService
    {
        StaffStatistic StaffStatistic(Staff staff, int yearId, int semesterId = 0, int gradeId = 0);
        List<StaffStatistic> StaveStatistic(List<Staff> stave, int yearId, int gradeId = 0, int semesterId = 0);
        List<StaffStatistic> StaveStatistic(int yearId, int departmentId = 0, int gradeId = 0, int semesterId = 0);
        List<StaffStatistic> CalculateMoney(List<StaffStatistic> data);
        FacultyAverage FacultyAverage(int yearId, int departmentId);
        List<StaffStatistic> ApplyFacultyComplementRatio(List<StaffStatistic> s, int staffFilter);
    }
}