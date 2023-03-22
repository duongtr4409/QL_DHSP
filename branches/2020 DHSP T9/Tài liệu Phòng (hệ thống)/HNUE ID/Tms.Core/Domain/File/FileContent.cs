using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Script.Serialization;
using Ums.Core.Base;
using Ums.Core.Domain.Users;

namespace Ums.Core.Domain.File
{
    [Table("File_Content")]
    public class FileContent : BaseEntity
    {
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string FileUrl { get; set; }
        public int CreatorId { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public virtual User Creator { get; set; }
        [JsonIgnore]
        [ScriptIgnore]
        public virtual ICollection<FileAccess> Accesses { get; set; }
    }
}