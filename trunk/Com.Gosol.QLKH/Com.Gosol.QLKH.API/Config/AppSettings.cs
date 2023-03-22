using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.API.Config
{
    public class AppSettings
    {
        public string ConnectionString { get; set; }
        public string ClientID { get; set; }
        public double NumberDateExpire { get; set; }
        public string AudienceSecret { get; set; }
        public string ApiUrl { get; set; }

        #region api in core
        // link api lấy từ core
        public string token { get; set; } // token api
        public string apptoken { get; set; } // appToken 

        // người dùng
        public string api_getusers { get; set; } // ds người dùng
        public string api_validate { get; set; } // đăng nhập
        public string api_changepassword { get; set; } // API: ĐỔI MẬT KHẨU NGƯỜI DÙNG		
        public string api_getuser_sso { get; set; } // thông tin người dùng

        // cán bộ
        public string api_getstave { get; set; } // ds cán bộ theo đơn vị
        public string api_getstaff { get; set; } // API: LẤY THÔNG TIN CÁN BỘ		
        public string api_getTitles { get; set; } // ds chức danh
        public string api_getPositions { get; set; } // ds chức vụ
        public string api_getDegrees { get; set; } // API: LẤY DANH SÁCH HỌC HÀM HỌC VỊ		



        // đơn vị trực thuộc
        public string api_getDepartments { get; set; } // đơn vị trực thuộc theo loại đơn vị loại =0 --> lấy tất cả
        public string api_getfaculties { get; set; } // ds khoa

        // nghiên cứu khoa học
        public string api_getCategories { get; set; } // API: LẤY DANH SÁCH DANH MỤC NHIỆM VỤ NCKH		
        public string api_getConversions { get; set; } // API: LẤY DANH SÁCH QUY ĐỔI NHIỆM VỤ NCKH		
        public string api_getTasks { get; set; }  // API: LẤY DANH SÁCH NHIỆM VỤ NCKH CỦA CÁN BỘ			
       

        // đào tạo
        public string api_getYears { get; set; }  // API: LẤY DANH SÁCH NĂM HỌC			

        #endregion


    }
}
