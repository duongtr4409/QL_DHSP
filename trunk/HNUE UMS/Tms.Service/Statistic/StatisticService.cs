using System.Collections.Generic;
using System.Linq;
using Hnue.Helper;
using Ums.Core.Common;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Personnel;
using Ums.Core.Entities.Statistic;
using Ums.Services.Calculate;
using Ums.Services.Data;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.System;
using Ums.Services.Task;

namespace Ums.Services.Statistic
{
    public class StatisticService : IStatisticService
    {
        private readonly IStaffCalculate _staffCalculate;
        private readonly ITaskReservedService _researchingReservedService;
        private readonly IGradeService _gradeService;
        private readonly ISettingService _settingService;
        private readonly IStaffClassifyService _staffClassifyService;
        private readonly IYearService _yearService;
        private readonly ITeachingService _teachingService;
        private readonly ResearchingService _researchingService;
        private readonly IWorkingService _workingService;
        private readonly IClassifyService _classifyService;
        private readonly IStaffService _staffService;
        private readonly IDepartmentService _departmentService;
        public StatisticService(IStaffCalculate staffCalculate, ITaskReservedService researchingReservedService, IGradeService gradeService, ISettingService settingService, IStaffClassifyService staffClassifyService, IYearService yearService, ITeachingService teachingService, ResearchingService researchingService, IWorkingService workingService, IClassifyService classifyService, IStaffService staffService, IDepartmentService departmentService)
        {
            _staffCalculate = staffCalculate;
            _researchingReservedService = researchingReservedService;
            _gradeService = gradeService;
            _settingService = settingService;
            _staffClassifyService = staffClassifyService;
            _yearService = yearService;
            _teachingService = teachingService;
            _researchingService = researchingService;
            _workingService = workingService;
            _classifyService = classifyService;
            _staffService = staffService;
            _departmentService = departmentService;
        }

        public StaffStatistic StaffStatistic(Staff staff, int yearId, int semesterId = 0, int gradeId = 0)
        {
            var duty = _staffCalculate.CalculateDutyTime(yearId, staff);
            var grade = _gradeService.Get(gradeId) ?? new Grade();
            var overSizeRatio = _settingService.GetValue(STANDARD.CLASS_SIZE_OVER_RATIO).ToDouble();
            var dutyRatio = _settingService.GetValue(STANDARD.CALCULATE_DUTY_TIME_RATIO).ToDouble();
            var overTimeRatio = _settingService.GetValue(STANDARD.OVERTIME_RATIO).ToDouble();
            var workFromResearchMax = _settingService.GetValue(Settings.PRACTICE_TRAINER_MAX_RESEARCH_TIME_USING).ToDouble();
            var classifyId = _staffClassifyService.GetClassify(yearId, staff.Id)?.ClassifyId;
            var year = _yearService.Get(yearId);
            var teaching = _teachingService.Gets(yearId, staffId: staff.Id, semesterId: semesterId, gradeId: gradeId, confirmed: 1, approved: 1).ToList();
            var researching = _researchingService.Gets(yearId, staffId: staff.Id, approvedOrPhrased: 1).ToList();
            var working = _workingService.Gets(yearId, staffId: staff.Id, approved: 1).ToList();
            var reserved = _researchingReservedService.GetReserved(staff.Id, yearId);
            return new StaffStatistic
            {
                Year = year,
                Classify = _classifyService.Get(classifyId) ?? new Classify(),
                Department = staff.Department,
                TeachingIn = staff.TeachingIn ?? staff.Department,
                Grade = grade,
                Staff = staff,
                DutyRatio = dutyRatio,
                TeachDuty = duty.Teaching,
                TeachReal = teaching.Sum(i => i.LessonTime),
                TeachDone = teaching.Where(i => !i.Paid).Sum(i => i.CalculateTeachingTime(overSizeRatio)),
                TeachPaid = teaching.Where(i => i.Paid).Sum(i => i.CalculateTeachingTime(overSizeRatio)),
                ResearchDuty = duty.Researching,
                ResearchDone = researching.Sum(i => i.CalculateResearchingTime()),
                WorkDuty = duty.Working,
                WorkDone = working.Sum(i => i.CalculateWorkingTime()),
                MaxWorkFromResearch = workFromResearchMax,
                ResearchReserved = reserved,
                OverTimeRatio = overTimeRatio
            };
        }

        public List<StaffStatistic> StaveStatistic(List<Staff> stave, int yearId, int gradeId = 0, int semesterId = 0)
        {
            return stave.Select(i => StaffStatistic(i, yearId, semesterId, gradeId)).ToList();
        }

        public List<StaffStatistic> StaveStatistic(int yearId, int departmentId = 0, int gradeId = 0, int semesterId = 0)
        {
            var stave = _staffService.GetTrainerIn(departmentId, moved: 2, retired: 2).ToList();
            return StaveStatistic(stave, yearId, gradeId, semesterId);
        }

        public List<StaffStatistic> CalculateMoney(List<StaffStatistic> data)
        {
            var standardMoney = _settingService.GetValue(STANDARD.STANDARD_MONEY_PER_HOUR).ToDouble();
            var v1 = _settingService.GetValue(STANDARD.FACULTY_TEACHING_TIME_LEVEL_1).ToDouble();
            var v2 = _settingService.GetValue(STANDARD.FACULTY_TEACHING_TIME_LEVEL_2).ToDouble();
            foreach (var d in data.Select(i => i.Department.Id).Distinct().ToArray())
            {
                var facultyAverage = data.Where(i => i.Staff.DepartmentId == d).Sum(i => i.OverTime) / _staffService.GetTrainerIn(d, moved: 2, retired: 2).Count();
                facultyAverage = facultyAverage >= v2 ? v2 : v1;
                foreach (var s in data.Where(i => i.Staff.DepartmentId == d))
                {
                    s.CalculateMoney(standardMoney, facultyAverage);
                }
            }
            return data;
        }

        public List<StaffStatistic> ApplyFacultyComplementRatio(List<StaffStatistic> s, int staffFilter)
        {
            //Move or retired
            if (staffFilter == 4)
            {
                foreach (var ss in s)
                {
                    ss.DepartmentRatio = 1;
                }
                return s;
            }
            //Normal
            var dutyRatio = _settingService.GetValue(STANDARD.CALCULATE_DUTY_TIME_RATIO).ToDouble();
            foreach (var d in s.Select(i => i.Staff.TeachingInId).Distinct().ToArray())
            {
                var data = s.Where(i => i.Staff.TeachingInId == d || i.Staff.DepartmentId == d).ToList();
                var g1 = data.Sum(i => i.TeachDone - i.ResearchComplement - i.WorkComplement);
                List<Staff> stave;
                switch (staffFilter)
                {
                    case 2:
                        stave = _staffService.GetLecturerIn(d, 2, 2).ToList();
                        break;
                    case 3:
                        stave = _staffService.GetTeacherIn(d, 2, 2).ToList();
                        break;
                    default:
                        stave = _staffService.GetTrainerIn(d, 2, 2).ToList();
                        break;
                }
                var g2 = stave.Sum(i => _staffCalculate.CalculateDutyTime(s.FirstOrDefault()?.Year.Id ?? 0, i).Teaching) * dutyRatio;
                var k = 0.0;
                if (g1 > g2)
                {
                    var g = g1 - g2;
                    var x = data.Sum(i => i.TeachOver);
                    k = g / x;
                }
                foreach (var ss in s.Where(i => i.Staff.TeachingInId == d))
                {
                    ss.DepartmentRatio = k;
                }
            }
            return s;
        }

        public FacultyAverage FacultyAverage(int yearId, int departmentId)
        {
            var stave = _staffService.GetTrainerIn(departmentId, moved: 2, retired: 2).ToList();
            var s = StaveStatistic(stave, yearId);
            return new FacultyAverage
            {
                Name = _departmentService.Get(departmentId).Name,
                Stave = stave.Count,
                TotalOvertime = s.Sum(i => i.OverTime),
                TotalMoney = s.Sum(i => i.Money),
                TotalTeachWork = s.Sum(i => i.TeachDone + i.TeachPaid)
            };
        }
    }
}