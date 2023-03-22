using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Report
{
    [Table("Report_Type")]
    public class ReportType : BaseEntity
    {
        public string Name { get; set; }
        public string Template { get; set; }
        public string File { get; set; }
    }
}