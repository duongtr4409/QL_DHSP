using System;
using System.Web;

namespace Com.Gosol.QLKH.Security
{
    public enum ChucNangEnum
    {
        //// Quản trị hệ thống
        //HT_QuanTriHeThong=100,
        //HT_QuanLyTaiKhoan = 101,
        //HT_PhanQuyen = 102,
        //HT_CauHinh = 103,
        //HT_ThamSoHeThong = 104,
        //HT_NhatKyHeThong = 105,
        //HT_HuongDanSuDung = 106,
        //HT_DanhMucCoQuan = 107,
        //HT_DanhMucChucVu = 108,
        //HT_DanhMucTrangThaiCongViec = 109,


        //// Nhiệm vụ

        //NV_DanhSachNhiemVu = 201,
        //NV_DanhSachCongViec = 202,
        //NV_PhanCongCongViec = 203,
        //NV_XuLyCongViec = 204,


        //// Báo cáo, tra cứu
        //BC_BaoCao = 301,
        //TC_TraCuu = 401,


        ////QLKH
        //CheckIn_CheckOut =200,
        //BaoCao=300

        // -------- BEGIN OLD ---------
        //ly_lich_khoa_hoc = 102,
        //ql_de_xuat = 103,
        //ql_de_tai = 104,
        //bao_cao = 106,
        //ql_bieu_mau = 107,
        //ql_thong_bao = 108,
        //thuyet_minh_de_tai = 109,
        //------- END --------
        ly_lich_khoa_hoc = 217,
        ql_de_xuat = 212,
        ql_de_tai = 213,
        bao_cao = 214,
        ql_bieu_mau = 215,
        ql_thong_bao = 216,
        thuyet_minh_de_tai = 109,

        quan_tri_he_thong = 200, // admin
        ht_tai_khoan = 201,     // admin
        phan_quyen = 202,       // admin
        tham_so = 203,          // admin
        dm_cap_de_tai = 204,    // admin
        dm_linh_vuc = 205,      // admin
        dm_loai_hinh_nghien_cuu = 206,  // admin
        dm_loai_hoi_dong = 207, // admin
        dm_trang_thai = 208,    // admin
        dm_loai_ket_qua = 209, // admin
        don_vi_nghien_cuu = 210, // admin
        ql_hoi_dong = 211,


        ql_nha_khoa_hoc = 901,          //QLKH
        chi_tiet_nha_khoa_hoc = 902,    //QLKH
        ql_toan_truong = 903,           
        ql_don_vi = 904,
        de_xuat = 905,
        duyet_de_xuat = 906,


    }

    public class ChucNangEx
    {
        public ChucNangEnum ChucNang { get; set; }
        public AccessLevel AccLevel { get; set; }
    }
    public enum DieuKienEnum
    {
        or = 1,
        and = 2,
    }
};
