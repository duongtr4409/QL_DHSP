using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.Models.QLKH
{
    public class CategorieModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int ParentId { get; set; }
        public int CategoryId { get; set; }
        public object Shallowcopy()
        {
            return this.MemberwiseClone();
        }
    }

    public class CategoriesModel : CategorieModel
    {
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public int ParentId { get; set; }
        public List<CategoriesModel> Children { get; set; }
      
    }

    public class CoreDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }                 // Ngày sinh
        public int TitleId { get; set; }                       //ID Chức danh
        public string Code { get; set; }                       // Mã cán bộ
        public int DegreeId { get; set; }                      // ID Học hàm, học vị
        public int DepartmentId { get; set; }                  // ID Đơn vị
        public string Gender { get; set; }                     // Giới tính
        public int TeachingInId { get; set; }                  // ID đơn vị giảng dạy kiêm nhiệm
        public bool IsMoved { get; set; }                      // Cán bộ đã chuyển công tác
        public bool IsRetired { get; set; }                    // Cán bộ đã nghỉ hưu
        public bool IsProbation { get; set; }                  // Cán bộ đang thử việc
        public List<int> PositionIds { get; set; }             // Danh sách ID chức vụ kiêm nhiệm

        public string Email { get; set; }
        public string Username { get; set; }
        public int StaffId { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string Fax { get; set; }
        public string FName
        {
            get
            {
                if (Name == null || Name == "" || Name == string.Empty)
                {
                    return "";
                }
                var cr = Name.TrimEnd().Split(' ');
                if (cr == null || cr.Length < 1)
                {
                    return "";
                }
                if (cr.LastOrDefault() == null)
                {
                    return "";
                }
                if (cr.LastOrDefault().Length > 0)
                {
                    return cr.LastOrDefault().Substring(0, 1);
                }
                else return "";
            }
        }
    }

    public class Tasks
    {
        public int Id { get; set; }  // ID Nhiệm vụ
        public string Name { get; set; }  // Tên nhiệm vụ
        public int Quantity { get; set; }  // Số lượng
        public int Members { get; set; }  // Số thành viên tham gia
        public List<string> Attachs { get; set; }  // Danh sách minh chứng
        public string ConversionName { get; set; }  // Tên nhiệm vụ quy đổi
        public int ConversionId { get; set; }  // ID nhiệm vụ quy đổi
        public int StartYear { get; set; }  // Năm bắt đầu thực hiện
        public int YearId { get; set; }       //ID năm học
    }

    public class Year
    {
        public int Id { get; set; }    // ID Năm học
        public DateTime From { get; set; }     //   Ngày bắt đầu
        public DateTime To { get; set; }// Ngày kết thúc
    }

}
