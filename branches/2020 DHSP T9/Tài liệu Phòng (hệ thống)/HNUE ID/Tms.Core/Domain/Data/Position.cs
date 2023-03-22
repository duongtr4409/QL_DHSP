using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_Position")]
    public class Position : BaseEntity
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public double Ratio { get; set; }
        public double Allowance { get; set; }
        public double Quota { get; set; } = 1;
        public bool IsPublic { get; set; }
    }
}