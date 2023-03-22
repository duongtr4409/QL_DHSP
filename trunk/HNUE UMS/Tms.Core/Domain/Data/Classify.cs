using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;
using Ums.Core.Domain.Personnel;

namespace Ums.Core.Domain.Data
{
    [Table("Data_Classify")]
    public class Classify : BaseEntity
    {
        public string Name { get; set; }
        public double Ratio { get; set; }
        [JsonIgnore]
        public virtual ICollection<StaffClassify> StaffClassifies { get; set; }
    }
}
