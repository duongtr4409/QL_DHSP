using System.Linq;
using Hnue.Helper;
using Ums.Core.Common;
using Ums.Core.Domain.Personnel;
using Ums.Core.Entities.Task;
using Ums.Services.Conversion;
using Ums.Services.Data;
using Ums.Services.System;
using ResearchingService = Ums.Services.Task.ResearchingService;
using ITeachingService = Ums.Services.Task.ITeachingService;
using IWorkingService = Ums.Services.Task.IWorkingService;

namespace Ums.Services.Calculate
{
    public class StaffCalculate : IStaffCalculate
    {
        private readonly IStandardService _standardService;
        private readonly IYearService _yearService;
        private readonly ISettingService _settingService;
        private readonly ITeachingService _teachingService;
        private readonly ResearchingService _researchingService;
        private readonly IWorkingService _workingService;

        public StaffCalculate(IStandardService standardService, IYearService yearService, ISettingService settingService, ITeachingService teachingService, ResearchingService researchingService, IWorkingService workingService)
        {
            _standardService = standardService;
            _yearService = yearService;
            _settingService = settingService;
            _teachingService = teachingService;
            _researchingService = researchingService;
            _workingService = workingService;
        }

        public DutyTime CalculateDutyTime(int yearId, Staff staff)
        {
            var bellowYear = _settingService.GetValue(STANDARD.LECTURER_BELLOW_YEAR).ToInt();
            var bellowYearRatio = _settingService.GetValue(STANDARD.LECTURER_BELLOW_YEAR_RATIO).ToDouble();
            var probationRatio = _settingService.GetValue(STANDARD.PROBATION_RATIO).ToDouble();
            var standards = _standardService.Gets().ToList();
            var year = _yearService.Get(yearId);
            return staff.CalculateDutyTime(standards, year, bellowYear, bellowYearRatio, probationRatio);
        }

        public double CalculateTeachingCount(int yearId, int staffId, int gradeId = 0, int paid = 0, int approved = 0, int confirmed = 0)
        {
            return _teachingService.Gets(yearId, gradeId: gradeId, staffId: staffId, approved: approved, confirmed: confirmed).Count();
        }

        public double CalculateTeachingTime(int yearId, int staffId, int gradeId = 0, int paid = 0, int approved = 0, int confirmed = 0)
        {
            var overSizeRatio = _settingService.GetValue(STANDARD.CLASS_SIZE_OVER_RATIO).ToDouble();
            return _teachingService.Gets(yearId, gradeId: gradeId, staffId: staffId, approved: approved, paid: paid, confirmed: confirmed).ToList().Sum(i => i.CalculateTeachingTime(overSizeRatio));
        }

        public double CalculateResearchingCount(int yearId, int staffId, int approved = 0)
        {
            return _researchingService.Gets(yearId, approvedOrPhrased: approved, staffId: staffId).Count();
        }

        public double CalculateResearchingTime(int yearId, int staffId, int approved = 0)
        {
            return _researchingService.Gets(yearId, staffId: staffId, approved: approved).ToList().Sum(i => i.CalculateResearchingTime());
        }

        public double CalculateWorkingCount(int yearId, int staffId, int approved = 0)
        {
            return _workingService.Gets(yearId, approved: approved, staffId: staffId).Count();
        }

        public double CalculateWorkingTime(int yearId, int staffId, int approved = 0)
        {
            return _workingService.Gets(yearId, staffId: staffId, approved: approved).ToList().Sum(i => i.CalculateWorkingTime());
        }
    }
}