using System;
using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;
using Ums.Core.Domain.Organize;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Connect
{
    [Table("Connect_Feedback")]
    public class Feedback : BaseEntity
    {
        public int StaffId { get; set; }
        public int DepartmentId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Response { get; set; }
        public int ResponserId { get; set; }
        public DateTime Updated { get; set; } = DateTime.Now;
        public DateTime Responsed { get; set; } = DateTime.Now;
        public virtual Staff Staff { get; set; }
        [ForeignKey(nameof(ResponserId))]
        public virtual Staff Responser { get; set; }
        public virtual Department Department { get; set; }
    }
}