using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Ums.Core.Domain.Organize;
using Ums.Core.Types;

namespace Ums.Models.Organize
{
    [NotMapped]
    public class DepartmentModel : Department
    {
        [Required]
        public new string Code { get; set; }
        [Required]
        public new string Name { get; set; }
        public IEnumerable<SelectListItem> Types = new List<SelectListItem>
        {
            new SelectListItem{Value = ((int)DepartmentTypes.Faculty).ToString(), Text = "Khoa & Bộ môn trực thuộc"},
            new SelectListItem{Value = ((int)DepartmentTypes.Division).ToString(), Text = "Phòng ban"},
            new SelectListItem{Value = ((int)DepartmentTypes.Center).ToString(), Text = "Trung tâm"},
            new SelectListItem{Value = ((int)DepartmentTypes.Institute).ToString(), Text = "Trung tâm NCKH"},
            new SelectListItem{Value = ((int)DepartmentTypes.School).ToString(), Text = "Trường học"}
        };
    }
}