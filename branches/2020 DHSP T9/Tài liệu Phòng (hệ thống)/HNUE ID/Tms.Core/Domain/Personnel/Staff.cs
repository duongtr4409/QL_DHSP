using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;
using Ums.Core.Domain.Data;
using Ums.Core.Domain.Organize;

namespace Ums.Core.Domain.Personnel
{
    [Table("Personnel_Staff")]
    public class Staff : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int TeachingInId { get; set; }
        public int TitleId { get; set; }
        public string Gender { get; set; }
        public DateTime Birthday { get; set; }
        public int DegreeId { get; set; }
        public int SalaryLevel { get; set; }
        public DateTime SalaryRaiseOn { get; set; }
        public int FrameExceeded { get; set; }
        public int StartYear { get; set; }
        public int StartMonth { get; set; }
        public bool IsProbation { get; set; }
        public int TrialMonth { get; set; }
        public int TrialYear { get; set; }
        public string AccountNumber { get; set; }
        public bool IsRetired { get; set; }
        public bool IsMoved { get; set; }
        public string MovedTo { get; set; }
        public string TaxNumber { get; set; }
        public DateTime RetireOrMoveDate { get; set; }
        public virtual Degree Degree { get; set; }
        public virtual Title Title { get; set; }
        public virtual Department Department { get; set; }
        [JsonIgnore]
        public virtual Department TeachingIn { get; set; }
        [JsonIgnore]
        public virtual ICollection<StaffPosition> Positions { get; set; }
    }
}