using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreAPI.Entity
{
    public class clsRespon
    {
        public string success { get; set; }
        public object data { get; set; }
        public string message { get; set; }
        public int total { get; set; }
    }
}