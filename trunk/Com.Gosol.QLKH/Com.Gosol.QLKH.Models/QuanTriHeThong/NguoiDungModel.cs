using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class NguoiDungModel
    {
        public int NguoiDungID { get; set; } = 0;
        public string TenNguoiDung { get; set; }
        public string MatKhau { get; set; }
        public string GhiChu { get; set; }
        public int TrangThai { get; set; }
        public int CanBoID { get; set; }
        public int CoQuanID { get; set; }
        public string TenCanBo { get; set; }
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public string RoleName { get; set; }
        public int GioiTinh { get; set; }
        public string Token { get; set; }
        public int CapCoQuan { get; set; }
        public int VaiTro { get; set; }
        public DateTime? expires_at { get; set; }
        public string AnhHoSo { get; set; }
        public int QuanLyThanNhan { get; set; }
        public int CoQuanSuDungPhanMem { get; set; }
        public NguoiDungModel() { }
        public NguoiDungModel(int NguoiDungID, string TenNguoiDung)
        {
            this.NguoiDungID = NguoiDungID;
            this.TenNguoiDung = TenNguoiDung;
        }
    }


    // model từ core trả về
    public class NguoiDungCoreModel
    {
        public int Id { get; set; } = 0;
        public string Email { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string UserKey { get; set; }
        public int StaffId { get; set; }
        public int? DepartmentId { get; set; }
        public List<int> PositionIds { get; set; }
    }
}
