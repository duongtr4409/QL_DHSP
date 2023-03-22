using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_Grade")]
    public class Grade : BaseEntity
    {
        public string Name { get; set; }
        public int Order { get; set; }
        public int ClassOverSize { get; set; }
    }
}