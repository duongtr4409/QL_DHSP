﻿using Com.Gosol.QLKH.Models.DanhMuc;
using Com.Gosol.QLKH.Models.QLKH;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Com.Gosol.QLKH.Models.QuanTriHeThong
{
    public class HeThongCanBoModel
    {
        public bool LaCanBoTrongTruong { get; set; } = false; // true - trong trường, false - Ngoài trường
        [Required]
        public int CanBoID { get; set; }
        public string TenCanBo { get; set; }
        public string FName { get; set; }
        public DateTime? NgaySinh { get; set; }
        public int? GioiTinh { get; set; }
        public string GioiTinhStr { get; set; }
        public string DiaChi { get; set; }
        public string TenNguoiDung { get; set; }
        public int? ChucVuID { get; set; }
        public int? QuyenKy { get; set; }
        public List<int> ListNhomNguoiDungID { get; set; }
        public string Email { get; set; }
        public string DienThoai { get; set; }
        public int? PhongBanID { get; set; }
        public int? CoQuanID { get; set; }
        public int? RoleID { get; set; }
        public int? QuanTridonVi { get; set; }
        public int? CoQuanCuID { get; set; }
        public int? CanBoCuID { get; set; }
        public int? XemTaiLieuMat { get; set; }
        public string AnhHoSo { get; set; }
        public int? IsStatus { get; set; }
        public string HoKhau { get; set; }
        public string MaCB { get; set; }
        public int ThanNhanID { get; set; }
        public string HoTenThanNhan { get; set; }
        public int? CapQuanLy { get; set; }
        public List<int> DanhSachChucVuID { get; set; }
        public List<string> DanhSachTenChucVu { get; set; }
        public int? TrangThaiID { get; set; }
        public int? NguoiDungID { get; set; }
        public int? VaiTro { get; set; }
        public string TenCoQuan { get; set; }
        public int CapCoQuanID { get; set; }
        public string CMND { get; set; }
        public string NoiCap { get; set; }
        public DateTime? NgayCap { get; set; }
        public bool? LaLeTan { get; set; }
        public int ChucDanhID { get; set; }
        public string TenChucDanh { get; set; }
        public int HocHamHocViID { get; set; }
        public string TenHocHamHocVi { get; set; }
        public string ChucDanhKhoaHocIDStr { get; set; }
        public string ChucDanhHanhChinhIDStr { get; set; }
        public List<int> ChucDanhKhoaHoc { get; set; }
        public List<int> ChucDanhHanhChinh { get; set; }
        public string CoQuanCongTac { get; set; }
        public string DiaChiCoQuan { get; set; }
        public string DienThoaiDiDong { get; set; }
        public string Fax { get; set; }
        public Boolean? LaChuyenGia { get; set; }
        public string Url { get; set; }
        public List<FileDinhKemModel> FileDinhKem { get; set; }
        public string TenChucVu { get; set; }
        public string TenDonViCongTac { get; set; }
        public HeThongCanBoModel() { }
        public HeThongCanBoModel(int CanBoID, string TenCanBo, DateTime NgaySinh, int GioiTinh, string DiaChi, int ChucVuID, int QuyenKy, string Email, string DienThoai,
            int PhongBanID, int CoQuanID, int RoleID, int QuanTridonVi, int CoQuanCuID, int CanBoCuID, int XemTaiLieuMat, string AnhHoSo, string HoKhau,
            string MaCB, int CapQuanLy, int TrangThaiID, int NguoiDungID, int VaiTro, int CapCoQuanID)
        {
            this.CanBoID = CanBoID;
            this.TenCanBo = TenCanBo;
            this.NgaySinh = NgaySinh;
            this.GioiTinh = GioiTinh;
            this.DiaChi = DiaChi;
            this.ChucVuID = ChucVuID;
            this.QuyenKy = QuyenKy;
            this.Email = Email;
            this.DienThoai = DienThoai;
            this.PhongBanID = PhongBanID;
            this.CoQuanID = CoQuanID;
            this.RoleID = RoleID;
            this.QuanTridonVi = QuanTridonVi;
            this.CoQuanCuID = CoQuanCuID;
            this.CanBoCuID = CanBoCuID;
            this.XemTaiLieuMat = XemTaiLieuMat;
            //this.IsStatus = IsStatus;
            this.AnhHoSo = AnhHoSo;
            this.HoKhau = HoKhau;
            this.MaCB = MaCB;
            this.CapQuanLy = CapQuanLy;
            this.TrangThaiID = TrangThaiID;
            this.NguoiDungID = NguoiDungID;
            this.VaiTro = VaiTro;
            this.CapCoQuanID = CapCoQuanID;
        }


    }
    public class HeThongCanBoPartialModel : HeThongCanBoModel
    {
        public string TenChucVu { get; set; }

        public string TenTrangThai { get; set; }
        public string TenCapQuanLy { get; set; }
        public List<string> NguyenNhan { get; set; }
    }
    public class HeThongCanBoShortModel : HeThongCanBoModel
    {
        //public int CanBoID { get; set; }
        //public int ThanNhanID { get; set; }
        //public string TenCanBo { get; set; }
        //public string HoTenThanNhan { get; set; }
        public string TenDotKeKhai { get; set; }
        public HeThongCanBoShortModel() { }
    }
    public class CanBoChuVu
    {
        public int CanBoID { get; set; }
        public int ChucVuID { get; set; }
        public bool KeKhaiHangNam { get; set; }
        public int CapQuanLy { get; set; }

        public int TrangThaiID { get; set; }
        public int CoQuanID { get; set; }
    }
    public class Files
    {
        public string files { get; set; }
        public Files()
        {


        }
        public Files(string files)
        {
            this.files = files;
        }
    }

    public class ThongTinDonViModel
    {
        public string TenCanBo { get; set; }
        public string TenCoQuan { get; set; }
        public string TenCoQuanCha { get; set; }
        public int CanBoID { get; set; }
        public int CoQuanID { get; set; }
        public int CoQuanChaID { get; set; }
    }
}
