using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Gosol.QLKH.Models
{
    public static class CacheKeys
    {
        // cán bộ, user
        public static string UserInCore { get { return "_UserInCore"; } } // người dùng
        public static string StaffInCore { get { return "_StaffInCore"; } } // cán bộ


        public static string Titles { get { return "_Titles"; } } // chức danh
        public static string Degrees { get { return "_Degrees"; } } // học hàm, học vị
        public static string Positions { get { return "_Positions"; } } // chức vụ


        // đơn vị
        public static string Departments { get { return "_Departments"; } } // đơn vị


        // khoa học
        public static string Categories { get { return "_Categories"; } } // nhiệm vụ khoa học
        public static string Conversions { get { return "_Conversions"; } } // nhiệm vụ khoa học

        public static List<CategoriesModel> lstCategories { get; set; }
        public static List<CategoriesModel> lstConversions { get; set; }
    }
}
