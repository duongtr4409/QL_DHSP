using System.Collections.Generic;
using System.Linq;
using Ums.Core.Domain.Conversion;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;
using Ums.Core.Domain.Report;
using Ums.Core.Entities.Shared;

namespace Ums.App.Helper
{
    public static class PairExtensions
    {
        public static List<IdName> ToPair(this IQueryable<ReportData> lst)
        {
            return lst.Select(i => new { i.Id, i.Name }).ToList().Select(i => new IdName(i.Id, i.Name)).ToList();
        }
        public static List<IdName> ToPair(this IQueryable<Department> lst)
        {
            return lst.Select(i => new { i.Id, i.Name }).ToList().Select(i => new IdName(i.Id, i.Name)).ToList();
        }
        public static List<IdName> ToPair(this IQueryable<Title> lst)
        {
            return lst.Select(i => new { i.Id, i.Name }).ToList().Select(i => new IdName(i.Id, i.Name)).ToList();
        }
        public static List<IdName> ToPair(this IQueryable<Staff> lst)
        {
            return lst.Select(i => new { i.Id, i.Name }).ToList().Select(i => new IdName(i.Id, i.Name)).ToList();
        }
        public static List<IdName> ToPair(this IQueryable<ConversionWorking> lst)
        {
            return lst.Select(i => new { i.Id, i.Name, i.Unit }).ToList().Select(i => new IdName(i.Id, i.Name, i.Unit)).ToList();
        }
        public static List<IdName> ToPair(this IQueryable<ConversionWorkingCategory> lst)
        {
            return lst.Select(i => new { i.Id, i.Name }).ToList().Select(i => new IdName(i.Id, i.Name)).ToList();
        }
    }
}