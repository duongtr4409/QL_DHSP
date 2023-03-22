using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
        public string AccessToken { get; set; }      
    }
}
