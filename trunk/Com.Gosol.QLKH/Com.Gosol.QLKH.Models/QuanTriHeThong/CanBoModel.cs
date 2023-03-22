using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class CanBoModel
    {
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public DateTime NgaySinh { get; set; }
        public string NgaySinhStr { get; set; }
        public int GioiTinh { get; set; }
        public int GioiTinhStr { get; set; }
        public string DiaChi { get; set; }
        public int ChuoiNhaThuocid { get; set; }
        public int NhaThuocid { get; set; }
        public int Khoid { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string TenChuoiNhaThuoc { get; set; }
        public string TenNhaThuoc { get; set; }
        public int LoaiHinhThuc { get; set; }
        public int NhaThuocParentid { get; set; }
    }
    public class NhanVienDTO
    {
        public int NhanVienid { get; set; }
        public string TenNhanVien { get; set; }

    }
    public class CanBoPartialModel : CanBoModel
    {
        public int ID { get; set; }

        public string Ten { get; set; }
    }

    public class CanBoCoreModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
        public int TitleId { get; set; }
        public string Code { get; set; }
        public int DegreeId { get; set; }
        public int DepartmentId { get; set; }
        public string Gender { get; set; }
        public int TeachingInId { get; set; }
        public List<int> PositionIds { get; set; }
        public string FName
        {
            get
            {
                if (Name == null || Name == "" || Name == string.Empty)
                {
                    return "";
                }
                var cr = Name.Split(' ');
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

    public class DonViCanBoModel : CanBoCoreModel
    {
        public int CoQuanID { get; set; }
        public List<DonViCanBoModel> Children { get; set; }
    }
}
