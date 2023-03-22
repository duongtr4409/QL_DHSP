using System.Linq;
using Ums.Core.Domain.Report;
using Ums.Services.Base;

namespace Ums.Services.Report
{
    public interface IReportService : IService<ReportData>
    {
        void RunStatistic(int id);
        string Download(int id);
        IQueryable<ReportData> Gets(int yearId, int typeId = 0);
        IQueryable<ReportData> GetByKey(int yearId, string reportKey);
        void LockToggle(int id);
    }
}