using System;
using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.Task;
using Ums.Core.Entities.Statistic;
using Ums.Core.Entities.Task;
using Ums.Core.Types;

namespace Ums.Services.Calculate
{
    public static class StaffHelper
    {
        /// Tính giờ chuẩn 1 nhiệm vụ giảng dạy
        public static double CalculateTeachingTime(this TaskTeaching ts, double overSizeRatio)
        {
            double r;
            var maxsize = ts.Grade.ClassOverSize;
            //Giảng lý thuyết
            if (ts.Conversion.TeachingTypeId == 1)
            {
                if (ts.Size > maxsize)
                {
                    var sizeRatio = 1 + (ts.Size - maxsize) * overSizeRatio;
                    if (ts.ConversionId == 25)
                    {
                        if (sizeRatio > 3)
                        {
                            sizeRatio = 3;
                        }
                    }
                    else
                    {
                        if (sizeRatio > 2)
                        {
                            sizeRatio = 2;
                        }
                    }
                    r = ts.LessonTime * sizeRatio * ts.Conversion.Ratio;
                }
                else
                {
                    r = ts.LessonTime * ts.Conversion.Ratio;
                }
            }
            //Giảng thực hành, thí nghiệm, bài tập
            else if (ts.Conversion.TeachingTypeId == 2 || ts.Conversion.TeachingTypeId == 3)
            {
                r = ts.LessonTime * ts.Conversion.Ratio * ts.Size;
            }
            else
            {
                r = ts.LessonTime * ts.Conversion.Ratio;
            }
            return r;
        }

        //Tính giờ chuẩn 1 nhiệm vụ khác
        public static double CalculateWorkingTime(this TaskWorking ws)
        {
            if (ws.Conversion == null || ws.Conversion.Amount <= 0) return 0;
            var r = ws.Conversion.Ratio * (ws.Amount / ws.Conversion.Amount);
            return r;
        }

        //Tính giờ chuẩn 1 nhiệm vụ NCKH
        public static double CalculateResearchingTime(this TaskResearching ws)
        {
            //if (!ws.Approved && !ws.PhaseCompleted)
            //{
            //    return 0;
            //}
            var c = ws.Conversion;
            if (c == null)
            {
                return 0;
            }
            var d = 1;
            if (c.HasMember)
            {
                d = ws.Members - ws.Conversion.MemberOffset;
                if (d < 1)
                {
                    d = 1;
                }
            }
            var result = ws.Quantity * ws.Conversion.Ratio / d;
            if (ws.Conversion.EquivalentQuantity > 1)
            {
                result /= ws.Conversion.EquivalentQuantity;
            }
            if (ws.Members > 1)
            {
                result *= ws.Conversion.Factor;
            }
            if (!ws.Approved && ws.PhaseCompleted)
            {
                result /= ws.WorkTime;
            }
            return result;
        }

        //Tính giờ chuẩn nghĩa vụ của 1 giảng viên
        public static DutyTime CalculateDutyTime(this Staff staff, IEnumerable<ConversionStandard> standards, Year year, int bellowYears, double belowYearRatio, double probationRatio)
        {
            var standardTime = standards.FirstOrDefault(i => i.TitleId == staff.TitleId);
            var result = new DutyTime();
            if (standardTime == null) return result;
            result.Teaching = standardTime.Teaching;
            result.Researching = standardTime.Researching;
            result.Working = standardTime.Working;
            //Tập sự
            if (staff.IsProbation)
            {
                result.Teaching *= probationRatio;
                result.Researching *= probationRatio;
                result.Working *= probationRatio;
            }
            //Công tác dưới x năm
            if (year.To.Year - staff.StartYear < bellowYears)
            {
                result.Teaching *= belowYearRatio;
                result.Researching *= belowYearRatio;
                result.Working *= belowYearRatio;
            }
            else if (year.To.Year - staff.StartYear == bellowYears)
            {
                var startDate = new DateTime(staff.StartYear + bellowYears, staff.StartMonth, 1);
                var calDays = Math.Abs((int)(year.From - startDate).TotalDays);
                var ratio = calDays / year.TotalDays;
                result.Teaching = result.Teaching * ratio * belowYearRatio + result.Teaching * (1 - ratio);
                result.Researching = result.Researching * ratio * belowYearRatio + result.Researching * (1 - ratio);
                result.Working = result.Working * ratio * belowYearRatio + result.Working * (1 - ratio);
            }
            //Xét nghỉ hưu, chuyển công tác
            if ((staff.IsRetired || staff.IsMoved) && staff.RetireOrMoveDate > year.From && staff.RetireOrMoveDate < year.To)
            {
                var calDays = (year.To - staff.RetireOrMoveDate).TotalDays;
                var ratio = calDays / year.TotalDays;
                result.Teaching -= result.Teaching * ratio;
                result.Researching -= result.Researching * ratio;
                result.Working -= result.Working * ratio;
            }
            //Kiêm nhiệm (lấy chức vụ có hệ số thấp nhất)
            var positions = staff.Positions.Where(i => i.Start <= year.To && i.End >= year.From).OrderBy(i => i.Start).ThenBy(i => i.End).ToArray();
            if (positions.Length > 0)
            {
                var dates = new List<DateTime> { year.From, year.To };
                foreach (var p in positions)
                {
                    if (p.Start > year.From)
                    {
                        dates.Add(p.Start);
                    }
                    if (p.End < year.To)
                    {
                        dates.Add(p.End);
                    }
                }
                dates = dates.OrderBy(i => i).Distinct().ToList();
                var segments = new List<Tuple<DateTime, DateTime, double>>();
                for (var i = 0; i < dates.Count - 1; i++)
                {
                    var start = dates[i];
                    var end = dates[i + 1];
                    var pos = positions.Where(j => j.Start < end && j.End > start).ToArray();
                    var ratio = pos.Any() ? pos.Min(j => j.Position.Quota) : 0;
                    segments.Add(new Tuple<DateTime, DateTime, double>(start, end, ratio));
                }
                var teaching = result.Teaching;
                var researching = result.Researching;
                var working = result.Working;
                var noPosRatio = segments.Where(i => i.Item3 <= 0).Sum(i => (i.Item2 - i.Item1).TotalDays) / year.TotalDays;
                result.Teaching *= noPosRatio;
                result.Researching *= noPosRatio;
                if (staff.Title.TitleTypeId == (int)TitleTypes.PracticeTrainer)
                {
                    result.Working *= noPosRatio;
                }
                foreach (var quota in from s in segments.Where(i => i.Item3 > 0) let posDays = (s.Item2 - s.Item1).TotalDays where posDays > 0 select posDays / year.TotalDays * s.Item3)
                {
                    result.Teaching += teaching * quota;
                    result.Researching += researching * quota;
                    if (staff.Title.TitleTypeId == (int)TitleTypes.PracticeTrainer)
                    {
                        result.Working += working * quota;
                    }
                }
            }
            //Xét nghỉ chế độ
            var vacations = staff.Vacations.Where(i => !i.IsDeleted && i.Start <= year.To && year.From <= i.End).ToList();
            foreach (var vacation in vacations)
            {
                int vacationDays;
                if (vacation.Start < year.From && vacation.End < year.To)
                {
                    vacationDays = (int)(vacation.End - year.From).TotalDays;
                }
                else if (vacation.Start > year.From && vacation.End > year.To)
                {
                    vacationDays = (int)(year.To - vacation.Start).TotalDays;
                }
                else
                {
                    vacationDays = Math.Min((int)year.TotalDays, vacation.Days);
                }
                var percent = (1 - vacation.Type.Ratio) * vacationDays / year.TotalDays;
                result.Teaching -= result.Teaching * percent;
                result.Researching -= result.Researching * percent;
                result.Working -= result.Working * percent;
            }
            return result;
        }

        //Tính tiền thừa giờ
        public static void CalculateMoney(this StaffStatistic ss, double moneyStandard, double facultyAverage)
        {
            var moneyTime = ss.MoneyTime;
            double v1 = moneyTime, v2 = 0;
            if (moneyTime >= facultyAverage)
            {
                v1 = facultyAverage;
                v2 = moneyTime - v1;
            }
            ss.Money = (v1 * 1.0 + v2 * 0.8) * (ss.DegreeRatio + 0.1 * ss.TitleRatio) * ss.Classify.Ratio * moneyStandard;
        }

        //Xét hoàn thành nhiệm vụ
        public static void CalculateDone(this StaffStatistic staffStat)
        {
            if (staffStat.Staff.Title.TitleTypeId == (int)TitleTypes.Lecturer)
            {
                staffStat.Done = staffStat.TeachDone + staffStat.TeachPaid >= staffStat.TeachDuty / 2 &&
                                 staffStat.ResearchDone + staffStat.ResearchReserved >= staffStat.ResearchDuty / 2 &&
                                 staffStat.WorkDone >= staffStat.WorkDuty / 2 &&
                                 staffStat.TeachDone + staffStat.TeachPaid + staffStat.ResearchDone + staffStat.ResearchReserved + staffStat.WorkDone >= staffStat.TeachDuty + staffStat.ResearchDuty + staffStat.WorkDuty;
            }
            else if (staffStat.Staff.Title.TitleTypeId == (int)TitleTypes.PracticeTrainer)
            {
                staffStat.Done = staffStat.TeachDone + staffStat.TeachPaid >= staffStat.TeachDuty / 2 &&
                                 staffStat.WorkDoneTotal >= staffStat.WorkDuty / 2 &&
                                 staffStat.TeachDone + staffStat.TeachPaid + staffStat.WorkDoneTotal >= staffStat.TeachDuty + staffStat.WorkDuty;
            }
        }

        public static void CalculateDone(this List<StaffStatistic> staffStat)
        {
            foreach (var staffStatistic in staffStat)
            {
                staffStatistic.CalculateDone();
            }
        }

        //Tính bảo lưu giờ NCKH 2 năm liên tiếp
        public static void CalculateResearchingReserved(this TaskReserved reserved)
        {
            var complement = reserved.PreviousReserved + reserved.TeachingDone - reserved.TeachingDuty + reserved.WorkingDone - reserved.WorkingDuty;
            var remain = reserved.ResearchingDone - reserved.ResearchingDuty;
            if (complement < 0)
            {
                remain += complement;
            }
            reserved.Reserved = remain > 0 ? remain : 0;
        }
    }
}