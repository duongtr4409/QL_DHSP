using Ums.Core.Domain.Personnel;
using Ums.Core.Entities.Task;

namespace Ums.Services.Calculate
{
    public interface IStaffCalculate
    {
        DutyTime CalculateDutyTime(int yearId, Staff staff);
        double CalculateTeachingCount(int yearId, int staffId, int gradeId = 0, int paid = 0, int approved = 0, int confirmed = 0);
        double CalculateTeachingTime(int yearId, int staffId, int gradeId = 0, int paid = 0, int approved = 0, int confirmed = 0);
        double CalculateResearchingCount(int yearId, int staffId, int approved = 0);
        double CalculateResearchingTime(int yearId, int staffId, int approved = 0);
        double CalculateWorkingCount(int yearId, int staffId, int approved = 0);
        double CalculateWorkingTime(int yearId, int staffId, int approved = 0);
    }
}