using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using Hnue.Helper;
using OfficeOpenXml;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.Report;
using Ums.Core.Entities.Statistic;
using Ums.Core.Types;
using Ums.Services.Base;
using Ums.Services.Calculate;
using Ums.Services.Organize;
using Ums.Services.Personnel;
using Ums.Services.Security;
using Ums.Services.Statistic;
using Ums.Services.Task;

namespace Ums.Services.Report
{
    public class ReportService : Service<ReportData>, IReportService
    {
        private readonly IDepartmentService _departmentService;
        private readonly ResearchingService _researchingService;
        private readonly IStatisticService _statisticService;
        private readonly IStaffService _staffService;
        private readonly ITypeService _typeService;
        public ReportService(DbContext context, IContextService contextService, IDepartmentService departmentService, ResearchingService researchingService, IStatisticService statisticService, IStaffService staffService, ITypeService typeService) : base(context, contextService)
        {
            _departmentService = departmentService;
            _researchingService = researchingService;
            _statisticService = statisticService;
            _staffService = staffService;
            _typeService = typeService;
        }

        public void RunStatistic(int id)
        {
            var s = Get(id);
            List<Staff> stave;
            switch (s.StaffFilter)
            {
                case 1:
                    stave = _staffService.GetTrainerIn(s.DepartmentId, 2, 2).ToList();
                    break;
                case 2:
                    stave = _staffService.GetLecturerIn(s.DepartmentId, 2, 2).ToList();
                    break;
                case 3:
                    stave = _staffService.GetTeacherIn(s.DepartmentId, 2, 2).ToList();
                    break;
                case 4:
                    stave = _staffService.GetTrainerIn(s.DepartmentId, movedOrRetired: 1).Where(i => i.RetireOrMoveDate <= s.Year.To && i.RetireOrMoveDate >= s.Year.From).ToList();
                    break;
                default:
                    stave = _staffService.GetTrainerIn(s.DepartmentId).ToList();
                    break;
            }
            stave = stave.OrderBy(i => i.Department.Name).ThenBy(i => i.Name).ToList();
            switch (_typeService.Get(s.TypeId).Template)
            {
                case "TOTAL_STATISTIC":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        data.CalculateDone();
                        if (s.ApplyFacultyRatio)
                        {
                            data = _statisticService.ApplyFacultyComplementRatio(data, s.StaffFilter).ToList();
                        }
                        s.Data = data.ToJson();
                    }
                    break;
                case "MONEY_STATISTIC":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        if (s.ApplyFacultyRatio)
                        {
                            data = _statisticService.ApplyFacultyComplementRatio(data, s.StaffFilter).ToList();
                        }
                        var money = _statisticService.CalculateMoney(data);
                        s.Data = money.ToJson();
                    }
                    break;
                case "MONEY_STATISTIC_DEDUCTED":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        if (s.ApplyFacultyRatio)
                        {
                            data = _statisticService.ApplyFacultyComplementRatio(data, s.StaffFilter).ToList();
                        }
                        var money = _statisticService.CalculateMoney(data);
                        var deducted = Get(s.MoneyReportId);
                        if (deducted == null) break;
                        var deduct = deducted.Data.CastJson<List<StaffStatistic>>();
                        foreach (var m in money)
                        {
                            m.MoneyDeducted = deduct.FirstOrDefault(i => i.Staff.Id == m.Staff.Id)?.Money ?? 0;
                        }
                        s.Data = money.ToJson();
                    }
                    break;
                case "MONEY_STATISTIC_DEDUCTED_UNDONE":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        if (s.ApplyFacultyRatio)
                        {
                            data = _statisticService.ApplyFacultyComplementRatio(data, s.StaffFilter).ToList();
                        }
                        var money = _statisticService.CalculateMoney(data);
                        var deducted = Get(s.MoneyReportId);
                        if (deducted == null) break;
                        var deduct = deducted.Data.CastJson<List<StaffStatistic>>();
                        foreach (var m in money)
                        {
                            m.MoneyDeducted = deduct.FirstOrDefault(i => i.Staff.Id == m.Staff.Id)?.Money ?? 0;
                        }
                        s.Data = money.Where(i => i.MoneyRemain < 0).ToJson();
                    }
                    break;
                case "FACULTY_OVERTIME_AVERAGE":
                    {
                        var data = new List<FacultyAverage>();
                        var departments = _departmentService.Gets(new[] { DepartmentTypes.Faculty }).Select(i => new { i.Id, i.Name }).ToArray();
                        foreach (var dept in departments)
                        {
                            var teachers = stave.Where(j => j.DepartmentId == dept.Id).ToList();
                            var stat = _statisticService.StaveStatistic(teachers, s.YearId);
                            var money = _statisticService.CalculateMoney(stat);
                            data.Add(new FacultyAverage
                            {
                                Name = dept.Name,
                                Stave = teachers.Count,
                                TotalTeachWork = stat.Sum(i => i.TeachDone + i.TeachPaid),
                                TotalMoney = money.Sum(i => i.Money),
                                TotalOvertime = stat.Sum(i => i.OverTime)
                            });
                        }
                        s.Data = data.Where(i => i.Stave > 0).ToList().ToJson();
                    }
                    break;
                case "COMPLEMENT_RATIO_STATISTIC":
                    {
                        var data = FacultyComplementRatio(s.ReportId).ToList();
                        s.Data = data.ToJson();
                    }
                    break;
                case "RESEARCHING_UNAPPROVED":
                    {
                        var data = ResearchingUnApproved(s.YearId).ToList();
                        s.Data = data.ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_1_IN_3_OVERTIME":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => i.ResearchingFinished && i.WorkFinished && i.TeachingFinished && i.OverTime > 0).ToList().ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_3_TASKS":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => !i.ResearchingFinished && !i.WorkFinished && !i.TeachingFinished).ToList().ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_TEACHING":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => !i.TeachingFinished).ToList().ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_RESEARCHING":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => !i.ResearchingFinished).ToList().ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_WORKING":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => !i.WorkFinished).ToList().ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_TEACHING_RESEARCHING":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => !i.TeachingFinished && !i.ResearchingFinished).ToList().ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_TEACHING_WORKING":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => !i.TeachingFinished && !i.WorkFinished).ToList().ToJson();
                    }
                    break;
                case "LECTURER_UNFINISHED_RESEARCHING_WORKING":
                    {
                        var data = _statisticService.StaveStatistic(stave, s.YearId, s.GradeId, s.SemesterId);
                        s.Data = data.Where(i => !i.ResearchingFinished && !i.WorkFinished).ToList().ToJson();
                    }
                    break;
            }
            s.Updated = DateTime.Now;
            s.Run = true;
            s.File = "";
            Update(s);
        }

        public string Download(int id)
        {
            var f = GetColumns(id, i => i.File);
            if (string.IsNullOrEmpty(f) || !File.Exists(HttpContext.Current.Server.MapPath(f)))
            {
                var s = Get(id);
                switch (s.Type.Template)
                {
                    case "TOTAL_STATISTIC":
                        {
                            var file = File.OpenRead(HttpContext.Current.Server.MapPath(s.Type.File));
                            using (var excel = new ExcelPackage(file))
                            {
                                file.Close();
                                file.Dispose();
                                int i = 9, j = 1;
                                var data = s.Data.CastJson<List<StaffStatistic>>() ?? new List<StaffStatistic>();
                                foreach (var t in data)
                                {
                                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.TeachingIn.ShortName;
                                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Staff.Title?.ShortName + ". " + t.Staff.Degree?.Name + ". " + t.Staff.Name;
                                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.Classify?.Name;
                                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.TeachDuty;
                                    excel.Workbook.Worksheets[1].Cells[i, 6].Value = t.TeachReal;
                                    excel.Workbook.Worksheets[1].Cells[i, 7].Value = t.TeachDone;
                                    excel.Workbook.Worksheets[1].Cells[i, 8].Value = t.TeachPaid;
                                    excel.Workbook.Worksheets[1].Cells[i, 9].Value = t.TeachComplement;
                                    excel.Workbook.Worksheets[1].Cells[i, 10].Value = t.ResearchDuty;
                                    excel.Workbook.Worksheets[1].Cells[i, 11].Value = t.ResearchReserved;
                                    excel.Workbook.Worksheets[1].Cells[i, 12].Value = t.ResearchDone;
                                    excel.Workbook.Worksheets[1].Cells[i, 13].Value = t.ResearchComplement;
                                    excel.Workbook.Worksheets[1].Cells[i, 14].Value = t.WorkDuty;
                                    excel.Workbook.Worksheets[1].Cells[i, 15].Value = t.WorkDoneTotal;
                                    excel.Workbook.Worksheets[1].Cells[i, 16].Value = t.WorkComplement;
                                    excel.Workbook.Worksheets[1].Cells[i, 17].Value = t.TotalDuty;
                                    excel.Workbook.Worksheets[1].Cells[i, 18].Value = t.TotalDone;
                                    excel.Workbook.Worksheets[1].Cells[i, 19].Value = t.OverTime;
                                    excel.Workbook.Worksheets[1].Cells[i, 20].Value = t.DepartmentRatio;
                                    excel.Workbook.Worksheets[1].Cells[i, 21].Value = t.MoneyTime;
                                    excel.Workbook.Worksheets[1].Cells[i, 22].Value = t.Done ? "✔" : "";
                                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                                    i++;
                                }
                                excel.Workbook.Worksheets[1].DeleteRow(i, 1);
                                excel.Workbook.Worksheets[1].Cells[i, 5].Value = Math.Round(data.Sum(k => k.TeachDuty), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 6].Value = Math.Round(data.Sum(k => k.TeachReal), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 7].Value = Math.Round(data.Sum(k => k.TeachDone), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 8].Value = Math.Round(data.Sum(k => k.TeachPaid), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 9].Value = Math.Round(data.Sum(k => k.TeachComplement), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 10].Value = Math.Round(data.Sum(k => k.ResearchDuty), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 11].Value = Math.Round(data.Sum(k => k.ResearchReserved), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 12].Value = Math.Round(data.Sum(k => k.ResearchDone), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 13].Value = Math.Round(data.Sum(k => k.ResearchComplement), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 14].Value = Math.Round(data.Sum(k => k.WorkDuty), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 15].Value = Math.Round(data.Sum(k => k.WorkDoneTotal), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 16].Value = Math.Round(data.Sum(k => k.WorkComplement), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 17].Value = Math.Round(data.Sum(k => k.TotalDuty), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 18].Value = Math.Round(data.Sum(k => k.TotalDone), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 19].Value = Math.Round(data.Sum(k => k.OverTime), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 21].Value = Math.Round(data.Sum(k => k.MoneyTime), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 22].Value = data.Count(k => k.Done);
                                //finished
                                excel.Workbook.Worksheets[1].Cells["Q4"].Value = "Ngày tạo: " + DateTime.Now.ToString("HH:mm dd-MM-yy");
                                excel.Workbook.Worksheets[1].Cells["A3"].Value = s.Name.ToUpper();
                                const string path = "/Files/ReportResults/";
                                var rootPath = HttpContext.Current.Server.MapPath(path);
                                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                                var filename = Common.RemoveNonAlphaNumberic(Common.UnicodeToAscii(s.Name)) + " " + Common.GetTimeString() + ".xlsx";
                                excel.SaveAs(new FileInfo(rootPath + filename));
                                s.File = path + filename;
                            }
                        }
                        break;
                    case "MONEY_STATISTIC":
                        {
                            var file = File.OpenRead(HttpContext.Current.Server.MapPath(s.Type.File));
                            using (var excel = new ExcelPackage(file))
                            {
                                file.Close();
                                file.Dispose();
                                int i = 7, j = 1;
                                var data = s.Data.CastJson<List<StaffStatistic>>() ?? new List<StaffStatistic>();
                                foreach (var t in data)
                                {
                                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.Staff.Code;
                                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Staff.Title?.ShortName + ". " + t.Staff.Degree.Name + ". " + t.Staff.Name;
                                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.TeachingIn.ShortName;
                                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.MoneyTime;
                                    excel.Workbook.Worksheets[1].Cells[i, 6].Value = t.DegreeRatio;
                                    excel.Workbook.Worksheets[1].Cells[i, 7].Value = t.TitleRatio;
                                    excel.Workbook.Worksheets[1].Cells[i, 8].Value = t.Classify.Ratio;
                                    excel.Workbook.Worksheets[1].Cells[i, 9].Value = t.Money;
                                    excel.Workbook.Worksheets[1].Cells[i, 10].Value = t.Staff.AccountNumber;
                                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                                    i++;
                                }
                                excel.Workbook.Worksheets[1].DeleteRow(i, 1);
                                excel.Workbook.Worksheets[1].Cells[i, 5].Value = Math.Round(data.Sum(k => k.MoneyTime), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 9].Value = Math.Round(data.Sum(k => k.Money), 2);
                                //finished
                                excel.Workbook.Worksheets[1].Cells["I4"].Value = "Ngày tạo: " + DateTime.Now.ToString("HH:mm dd-MM-yyyy");
                                excel.Workbook.Worksheets[1].Cells["A3"].Value = s.Name.ToUpper();
                                const string path = "/Files/ReportResults/";
                                var rootPath = HttpContext.Current.Server.MapPath(path);
                                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                                var filename = Common.RemoveNonAlphaNumberic(Common.UnicodeToAscii(s.Name)) + " " + Common.GetTimeString() + ".xlsx";
                                excel.SaveAs(new FileInfo(rootPath + filename));
                                s.File = path + filename;
                            }
                        }
                        break;
                    case "MONEY_STATISTIC_DEDUCTED":
                        {
                            var file = File.OpenRead(HttpContext.Current.Server.MapPath(s.Type.File));
                            using (var excel = new ExcelPackage(file))
                            {
                                file.Close();
                                file.Dispose();
                                int i = 7, j = 1;
                                var data = s.Data.CastJson<List<StaffStatistic>>() ?? new List<StaffStatistic>();
                                foreach (var t in data)
                                {
                                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.Staff.Code;
                                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Staff.Title?.ShortName + ". " + t.Staff.Degree.Name + ". " + t.Staff.Name;
                                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.TeachingIn.ShortName;
                                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.MoneyTime;
                                    excel.Workbook.Worksheets[1].Cells[i, 6].Value = t.DegreeRatio;
                                    excel.Workbook.Worksheets[1].Cells[i, 7].Value = t.TitleRatio;
                                    excel.Workbook.Worksheets[1].Cells[i, 8].Value = t.Classify.Ratio;
                                    excel.Workbook.Worksheets[1].Cells[i, 9].Value = t.Money;
                                    excel.Workbook.Worksheets[1].Cells[i, 10].Value = t.MoneyDeducted;
                                    excel.Workbook.Worksheets[1].Cells[i, 11].Value = t.MoneyRemain;
                                    excel.Workbook.Worksheets[1].Cells[i, 12].Value = t.Staff.AccountNumber;
                                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                                    i++;
                                }
                                excel.Workbook.Worksheets[1].DeleteRow(i, 1);
                                excel.Workbook.Worksheets[1].Cells[i, 5].Value = Math.Round(data.Sum(k => k.MoneyTime), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 9].Value = Math.Round(data.Sum(k => k.Money), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 10].Value = Math.Round(data.Sum(k => k.MoneyDeducted), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 11].Value = Math.Round(data.Sum(k => k.MoneyRemain), 2);
                                //finished
                                excel.Workbook.Worksheets[1].Cells["K4"].Value = "Ngày tạo: " + DateTime.Now.ToString("HH:mm dd-MM-yyyy");
                                excel.Workbook.Worksheets[1].Cells["A3"].Value = s.Name.ToUpper();
                                const string path = "/Files/ReportResults/";
                                var rootPath = HttpContext.Current.Server.MapPath(path);
                                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                                var filename = Common.RemoveNonAlphaNumberic(Common.UnicodeToAscii(s.Name)) + " " + Common.GetTimeString() + ".xlsx";
                                excel.SaveAs(new FileInfo(rootPath + filename));
                                s.File = path + filename;
                            }
                        }
                        break;
                    case "MONEY_STATISTIC_DEDUCTED_UNDONE":
                        {
                            var file = File.OpenRead(HttpContext.Current.Server.MapPath(s.Type.File));
                            using (var excel = new ExcelPackage(file))
                            {
                                file.Close();
                                file.Dispose();
                                int i = 7, j = 1;
                                var data = s.Data.CastJson<List<StaffStatistic>>() ?? new List<StaffStatistic>();
                                foreach (var t in data)
                                {
                                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.Staff.Code;
                                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Staff.Title?.ShortName + ". " + t.Staff.Degree.Name + ". " + t.Staff.Name;
                                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.TeachingIn.ShortName;
                                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.MoneyTime;
                                    excel.Workbook.Worksheets[1].Cells[i, 6].Value = t.DegreeRatio;
                                    excel.Workbook.Worksheets[1].Cells[i, 7].Value = t.TitleRatio;
                                    excel.Workbook.Worksheets[1].Cells[i, 8].Value = t.Classify.Ratio;
                                    excel.Workbook.Worksheets[1].Cells[i, 9].Value = t.Money;
                                    excel.Workbook.Worksheets[1].Cells[i, 10].Value = t.MoneyDeducted;
                                    excel.Workbook.Worksheets[1].Cells[i, 11].Value = t.MoneyRemain;
                                    excel.Workbook.Worksheets[1].Cells[i, 12].Value = t.Staff.AccountNumber;
                                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                                    i++;
                                }
                                excel.Workbook.Worksheets[1].DeleteRow(i, 1);
                                excel.Workbook.Worksheets[1].Cells[i, 5].Value = Math.Round(data.Sum(k => k.MoneyTime), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 9].Value = Math.Round(data.Sum(k => k.Money), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 10].Value = Math.Round(data.Sum(k => k.MoneyDeducted), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 11].Value = Math.Round(data.Sum(k => k.MoneyRemain), 2);
                                //finished
                                excel.Workbook.Worksheets[1].Cells["K4"].Value = "Ngày tạo: " + DateTime.Now.ToString("HH:mm dd-MM-yyyy");
                                excel.Workbook.Worksheets[1].Cells["A3"].Value = s.Name.ToUpper();
                                const string path = "/Files/ReportResults/";
                                var rootPath = HttpContext.Current.Server.MapPath(path);
                                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                                var filename = Common.RemoveNonAlphaNumberic(Common.UnicodeToAscii(s.Name)) + " " + Common.GetTimeString() + ".xlsx";
                                excel.SaveAs(new FileInfo(rootPath + filename));
                                s.File = path + filename;
                            }
                        }
                        break;
                    case "FACULTY_OVERTIME_AVERAGE":
                        {
                            var data = s.Data.CastJson<List<FacultyAverage>>();
                            var file = File.OpenRead(HttpContext.Current.Server.MapPath(s.Type.File));
                            using (var excel = new ExcelPackage(file))
                            {
                                file.Close();
                                file.Dispose();
                                int i = 7, j = 1;
                                foreach (var t in data)
                                {
                                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.Name;
                                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Stave;
                                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.TotalOvertime;
                                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.OverTimeAverage;
                                    excel.Workbook.Worksheets[1].Cells[i, 6].Value = t.TotalTeachWork;
                                    excel.Workbook.Worksheets[1].Cells[i, 7].Value = t.WorkAverage;
                                    excel.Workbook.Worksheets[1].Cells[i, 8].Value = t.TotalMoney;
                                    excel.Workbook.Worksheets[1].Cells[i, 9].Value = t.MoneyAverage;
                                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                                    i++;
                                }
                                excel.Workbook.Worksheets[1].DeleteRow(i, 1);
                                excel.Workbook.Worksheets[1].Cells[i, 3].Value = data.Sum(k => k.Stave);
                                excel.Workbook.Worksheets[1].Cells[i, 4].Value = Math.Round(data.Sum(k => k.TotalOvertime), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 5].Value = Math.Round(data.Sum(k => k.OverTimeAverage), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 6].Value = Math.Round(data.Sum(k => k.TotalTeachWork), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 7].Value = Math.Round(data.Sum(k => k.WorkAverage), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 8].Value = Math.Round(data.Sum(k => k.TotalMoney), 2);
                                excel.Workbook.Worksheets[1].Cells[i, 9].Value = Math.Round(data.Sum(k => k.MoneyAverage), 2);
                                //finished
                                excel.Workbook.Worksheets[1].Cells["H4"].Value = "Ngày in: " + DateTime.Now.ToString("HH:mm dd-MM-yyyy");
                                excel.Workbook.Worksheets[1].Cells["A3"].Value = s.Name.ToUpper();
                                const string path = "/Files/ReportResults/";
                                var rootPath = HttpContext.Current.Server.MapPath(path);
                                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                                var filename = Common.GetTimeString() + ".xlsx";
                                excel.SaveAs(new FileInfo(rootPath + filename));
                                s.File = path + filename;
                            }
                        }
                        break;
                        //case "COMPLEMENT_RATIO_STATISTIC":
                        //    {
                        //        s.File = CreateComplementReportFile(s.Data.CastJson<List<FacultyComplementRatio>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "RESEARCHING_UNAPPROVED":
                        //    {
                        //        s.File = CreateResearchingUnApproveReportFile(s.Data.CastJson<List<ResearchingUnApproved>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_1_IN_3_OVERTIME":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_3_TASKS":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_TEACHING":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_RESEARCHING":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_WORKING":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_TEACHING_RESEARCHING":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_TEACHING_WORKING":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                        //case "LECTURER_UNFINISHED_RESEARCHING_WORKING":
                        //    {
                        //        s.File = CreateTotalReportFile(s.Data.CastJson<List<StaffTask>>(), type, s.Name);
                        //    }
                        //    break;
                }
                Update(s);
                return s.File;
            }
            return f;
        }

        public IQueryable<ReportData> Gets(int yearId, int typeId = 0)
        {
            var lst = base.Gets();
            if (yearId > 0)
            {
                lst = lst.Where(i => i.YearId == yearId);
            }
            if (typeId > 0)
            {
                lst = lst.Where(i => i.TypeId == typeId);
            }
            return lst.OrderByDescending(i => i.Created);
        }

        public List<FacultyComplementRatio> FacultyComplementRatio(int reportId)
        {
            var report = Get(reportId).Data.CastJson<List<StaffStatistic>>();
            foreach (var r in report.Where(r => r.Staff.Title.TitleTypeId == (int)TitleTypes.PracticeTrainer))
            {
                r.ResearchDuty = r.ResearchDone = 0;
            }
            var departments = _departmentService.Gets(new[] { DepartmentTypes.Faculty }).Select(i => i.Id).ToArray();
            return (from d in departments
                    select report.Where(i => i.Staff.DepartmentId == d).ToList() into deptdata
                    let stave = deptdata.Select(i => i.Staff).Distinct().ToList()
                    let po = deptdata.Where(i => i.TeachDone + i.ResearchDone + i.WorkDoneTotal - i.TeachDuty - i.ResearchDuty - i.WorkDuty > 0).Sum(i => i.TeachDone + i.ResearchDone + i.WorkDoneTotal - i.TeachDuty - i.ResearchDuty - i.WorkDuty)
                    let tpo = deptdata.Where(i => i.TeachDone - i.TeachDuty > 0).Sum(i => i.TeachDone - i.TeachDuty)
                    select new FacultyComplementRatio
                    {
                        Name = deptdata.FirstOrDefault()?.Department?.Name ?? "",
                        Stave = stave.Count,
                        TeachingDuty = deptdata.Sum(i => i.TeachDuty),
                        TeachingDone = deptdata.Sum(i => i.TeachDone),
                        TeachingPersonOver = tpo,
                        FacultyDuty = deptdata.Sum(i => i.TeachDuty + i.ResearchDuty + i.WorkDuty),
                        FacultyDone = deptdata.Sum(i => i.TeachDone + i.ResearchDone + i.WorkDoneTotal),
                        PersonOver = po > 0 ? po : 0
                    }).ToList();
        }

        public IQueryable<ReportData> GetByKey(int yearId, string reportKey)
        {
            return Gets().Where(i => i.YearId == yearId && i.Type.Template == reportKey);
        }

        public void LockToggle(int id)
        {
            var r = Get(id);
            r.Locked = !r.Locked;
            Update(r);
        }

        public List<ResearchingUnApproved> ResearchingUnApproved(int yearId)
        {
            var departments = _departmentService.Gets(new[] { DepartmentTypes.Faculty }).Select(i => new { i.Id, i.Name }).OrderBy(i => i.Name).ToList();
            var report = new List<ResearchingUnApproved>();
            var researching = _researchingService.Gets(yearId, approved: 2).Select(i => new { i.Staff.DepartmentId, Staff = i.Staff.Name, i.Name, i.ApproveMessage }).ToList();
            foreach (var d in departments)
            {
                report.AddRange(researching.Where(i => i.DepartmentId == d.Id).Select(r => new ResearchingUnApproved
                {
                    Department = d.Name,
                    Staff = r.Staff,
                    Name = r.Name,
                    Reason = string.IsNullOrEmpty(r.ApproveMessage) ? "" : r.ApproveMessage
                }));
            }
            return report;
        }

        public string CreateResearchingUnApproveReportFile(List<ResearchingUnApproved> data, ReportType type, string name)
        {
            var file = File.OpenRead(HttpContext.Current.Server.MapPath(type.File));
            using (var excel = new ExcelPackage(file))
            {
                file.Close();
                file.Dispose();
                int i = 7, j = 1;
                foreach (var t in data)
                {
                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.Department;
                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Staff;
                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.Name;
                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.Reason;
                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                    i++;
                }
                excel.Workbook.Worksheets[1].DeleteRow(i, 1);
                //finished
                excel.Workbook.Worksheets[1].Cells["E4"].Value = "Ngày in: " + DateTime.Now.ToString("HH:mm dd-MM-yyyy");
                excel.Workbook.Worksheets[1].Cells["A3"].Value = name.ToUpper();
                const string path = "/Files/ReportResults/";
                var rootPath = HttpContext.Current.Server.MapPath(path);
                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                var filename = Common.GetTimeString() + ".xlsx";
                excel.SaveAs(new FileInfo(rootPath + filename));
                return path + filename;
            }
        }

        public string CreateFacultyAverageReportFile(List<FacultyAverage> data, ReportType type, string name)
        {
            var file = File.OpenRead(HttpContext.Current.Server.MapPath(type.File));
            using (var excel = new ExcelPackage(file))
            {
                file.Close();
                file.Dispose();
                int i = 7, j = 1;
                foreach (var t in data)
                {
                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.Name;
                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Stave;
                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.TotalOvertime;
                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.OverTimeAverage;
                    excel.Workbook.Worksheets[1].Cells[i, 6].Value = t.TotalTeachWork;
                    excel.Workbook.Worksheets[1].Cells[i, 7].Value = t.WorkAverage;
                    excel.Workbook.Worksheets[1].Cells[i, 8].Value = t.TotalMoney;
                    excel.Workbook.Worksheets[1].Cells[i, 9].Value = t.MoneyAverage;
                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                    i++;
                }
                excel.Workbook.Worksheets[1].DeleteRow(i, 1);
                excel.Workbook.Worksheets[1].Cells[i, 3].Value = data.Sum(k => k.Stave);
                excel.Workbook.Worksheets[1].Cells[i, 4].Value = Math.Round(data.Sum(k => k.TotalOvertime), 2);
                excel.Workbook.Worksheets[1].Cells[i, 5].Value = Math.Round(data.Sum(k => k.OverTimeAverage), 2);
                excel.Workbook.Worksheets[1].Cells[i, 6].Value = Math.Round(data.Sum(k => k.TotalTeachWork), 2);
                excel.Workbook.Worksheets[1].Cells[i, 7].Value = Math.Round(data.Sum(k => k.WorkAverage), 2);
                excel.Workbook.Worksheets[1].Cells[i, 8].Value = Math.Round(data.Sum(k => k.TotalMoney), 2);
                excel.Workbook.Worksheets[1].Cells[i, 9].Value = Math.Round(data.Sum(k => k.MoneyAverage), 2);
                //finished
                excel.Workbook.Worksheets[1].Cells["H4"].Value = "Ngày in: " + DateTime.Now.ToString("HH:mm dd-MM-yyyy");
                excel.Workbook.Worksheets[1].Cells["A3"].Value = name.ToUpper();
                const string path = "/Files/ReportResults/";
                var rootPath = HttpContext.Current.Server.MapPath(path);
                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                var filename = Common.GetTimeString() + ".xlsx";
                excel.SaveAs(new FileInfo(rootPath + filename));
                return path + filename;
            }
        }

        public string CreateComplementReportFile(List<FacultyComplementRatio> data, ReportType type, string name)
        {
            var file = File.OpenRead(HttpContext.Current.Server.MapPath(type.File));
            using (var excel = new ExcelPackage(file))
            {
                file.Close();
                file.Dispose();
                int i = 7, j = 1;
                foreach (var t in data)
                {
                    excel.Workbook.Worksheets[1].Cells[i, 1].Value = j++;
                    excel.Workbook.Worksheets[1].Cells[i, 2].Value = t.Name;
                    excel.Workbook.Worksheets[1].Cells[i, 3].Value = t.Stave;
                    excel.Workbook.Worksheets[1].Cells[i, 4].Value = t.TeachingDuty;
                    excel.Workbook.Worksheets[1].Cells[i, 5].Value = t.TeachingDone;
                    excel.Workbook.Worksheets[1].Cells[i, 6].Value = t.TeachingOver;
                    excel.Workbook.Worksheets[1].Cells[i, 7].Value = t.TeachingPersonOver;
                    excel.Workbook.Worksheets[1].Cells[i, 8].Value = t.TeachingRatio;
                    excel.Workbook.Worksheets[1].Cells[i, 9].Value = t.FacultyDuty;
                    excel.Workbook.Worksheets[1].Cells[i, 10].Value = t.FacultyDone;
                    excel.Workbook.Worksheets[1].Cells[i, 11].Value = t.FacultyOver;
                    excel.Workbook.Worksheets[1].Cells[i, 12].Value = t.PersonOver;
                    excel.Workbook.Worksheets[1].Cells[i, 13].Value = t.Ratio;
                    excel.Workbook.Worksheets[1].InsertRow(i + 1, 1, i);
                    i++;
                }
                //finished
                excel.Workbook.Worksheets[1].Cells["K4"].Value = "Ngày in: " + DateTime.Now.ToString("HH:mm dd-MM-yyyy");
                excel.Workbook.Worksheets[1].Cells["A3"].Value = name.ToUpper();
                const string path = "/Files/ReportResults/";
                var rootPath = HttpContext.Current.Server.MapPath(path);
                if (!Directory.Exists(rootPath)) Directory.CreateDirectory(rootPath);
                var filename = Common.GetTimeString() + ".xlsx";
                excel.SaveAs(new FileInfo(rootPath + filename));
                return path + filename;
            }
        }
    }
}