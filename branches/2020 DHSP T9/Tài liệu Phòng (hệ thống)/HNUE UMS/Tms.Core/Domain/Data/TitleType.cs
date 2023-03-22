using System.ComponentModel.DataAnnotations.Schema;
using Ums.Core.Base;

namespace Ums.Core.Domain.Data
{
    [Table("Data_TitleType")]
    public class TitleType : BaseEntity
    {
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}