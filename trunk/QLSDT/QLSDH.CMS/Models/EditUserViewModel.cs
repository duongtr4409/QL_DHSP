using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TEMIS.CMS.Models
{
    public class EditUserViewModel
    {
        public string userName { get; set; }

        //[Required(AllowEmptyStrings = false)]
        //[Display(Name = "Email")]
        //[EmailAddress]
        //public string Email { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
    }
}