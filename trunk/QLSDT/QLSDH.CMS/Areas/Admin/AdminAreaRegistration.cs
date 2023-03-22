using System.Web.Mvc;
using System.Web.Routing;
using TEMIS.CMS.Common;

namespace TEMIS.CMS.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                name: "ThiSinhTuyenSinh",
                url: "Admin/danh-sach-dang-ky-tuyen-sinh",
               defaults: new { controller = "ThiSinhTuyenSinh", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                name: "quyetdinh",
                url: "Admin/dang-ky-tuyen-sinh/lap-quyet-dinh",
               defaults: new { controller = "ThiSinhTuyenSinh", action = "HoSoXetTuyen", id = UrlParameter.Optional }
            );
            context.MapRoute(
               name: "xintieuban",
               url: "Admin/dang-ky-tuyen-sinh/xin-tieu-ban",
              defaults: new { controller = "ThiSinhTuyenSinh", action = "XinTieuBan", id = UrlParameter.Optional }
           );
            context.MapRoute(
              name: "xettuyen",
              url: "Admin/dang-ky-tuyen-sinh/xet-duyet-ncs",
             defaults: new { controller = "ThiSinhTuyenSinh", action = "XetTuyen", id = UrlParameter.Optional }
          );
            context.MapRoute(
         name: "ncsdetail",
         url: "Admin/ncs/chi-tiet/{id}",
        defaults: new { controller = "ThiSinhTuyenSinh", action = "Detail", id = UrlParameter.Optional }
     );
            context.MapRoute(
      name: "danhsachncstrungtuyen",
      url: "Admin/ncs-trung-tuyen",
     defaults: new { controller = "ThiSinhTuyenSinh", action = "DanhSachNCS", id = UrlParameter.Optional }
  );
            context.MapRoute(
     name: "phieutrungtuyencanhanh",
     url: "Admin/trung-tuyen-ca-nhan",
    defaults: new { controller = "ThiSinhTuyenSinh", action = "DanhSachNCSTrungTuyen", id = UrlParameter.Optional }
 );

            context.MapRoute(
                name: "dsncs",
                url: "Admin/danh-sach-ncs",
               defaults: new { controller = "HocVien", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
               name: "muchocphi",
               url: "Admin/ql-hoc-phi/muc-hoc-phi",
              defaults: new { controller = "QuanLyHocPhi", action = "MucHocPhi", id = UrlParameter.Optional }
           );
            context.MapRoute(
              name: "duyethocphi",
              url: "Admin/ql-hoc-phi/duyet-hoc-phi",
             defaults: new { controller = "QuanLyHocPhi", action = "ApproveList", id = UrlParameter.Optional }
          );
            context.MapRoute(
             name: "thongkehocphi",
             url: "Admin/ql-hoc-phi/thong-ke",
            defaults: new { controller = "QuanLyHocPhi", action = "Index", id = UrlParameter.Optional }
         );
            context.MapRoute(
             name: "qldiem",
             url: "Admin/ql-diem",
            defaults: new { controller = "QuanLyDiem", action = "Index", id = UrlParameter.Optional }
         );
            context.MapRoute(
            name: "qlctdaotao",
            url: "Admin/ql-diem/ql-chuong-trinh-dao-tao/{id}",
           defaults: new { controller = "QuanLyDiem", action = "QuanLyDT", id = UrlParameter.Optional }
        );
            context.MapRoute(
           name: "qldiemncs",
           url: "Admin/ql-diem/nhap-diem-ncs/{id}",
          defaults: new { controller = "QuanLyDiem", action = "DiemHP", id = UrlParameter.Optional }
       );
            context.MapRoute(
         name: "tracuudiem",
         url: "Admin/ql-diem/tra-cuu-diem-ncs",
        defaults: new { controller = "QuanLyDiem", action = "TraCuuDiem", id = UrlParameter.Optional }
     );
            context.MapRoute(
             name: "qlgvtrongtruong",
             url: "Admin/ql-gv-trong-truong",
            defaults: new { controller = "GiangVien", action = "Index", id = UrlParameter.Optional }
         );
            context.MapRoute(
             name: "qlgvngoaitruong",
             url: "Admin/ql-gv-ngoai-truong",
            defaults: new { controller = "GiangVien", action = "GiangVienNgoaiTruong", id = UrlParameter.Optional }
         );
            context.MapRoute(
             name: "themmoigvngoai",
             url: "Admin/ql-gv-ngoai-truong/them-moi-gv",
            defaults: new { controller = "GiangVien", action = "ThemMoiUser", id = UrlParameter.Optional }
         );
            context.MapRoute(
            name: "qlhp",
            url: "Admin/ql-hoc-phan",
           defaults: new { controller = "HocPhan", action = "Index", id = UrlParameter.Optional }
        );
            context.MapRoute(
           name: "qlkhohoc",
           url: "Admin/ql-khoa-hoc",
          defaults: new { controller = "KhoaHoc", action = "Index", id = UrlParameter.Optional }
       );
            context.MapRoute(
           name: "qlkhoa",
           url: "Admin/ql-khoa",
          defaults: new { controller = "Khoa", action = "Index", id = UrlParameter.Optional }
       );
            context.MapRoute(
           name: "qlnganh",
           url: "Admin/ql-nganh-dao-tao",
          defaults: new { controller = "NganhDaoTao", action = "Index", id = UrlParameter.Optional }
       );
            context.MapRoute(
           name: "qlchuyennganh",
           url: "Admin/ql-chuyen-nganh-dao-tao",
          defaults: new { controller = "ChuyenNganhDaoTao", action = "Index", id = UrlParameter.Optional }
       );
            context.MapRoute(
           name: "qlluanan",
           url: "Admin/nop-luan-an-ts",
          defaults: new { controller = "QuanLyThuVien", action = "Index", id = UrlParameter.Optional }
       );
            context.MapRoute(
           name: "qltaikhoanmoigiang",
           url: "Admin/ql-tai-khoan/gv-moi-giang",
          defaults: new { controller = "Taikhoan", action = "Teachers", id = UrlParameter.Optional }
       );
            context.MapRoute(
           name: "qltaikhoantrongtruong",
           url: "Admin/ql-tai-khoan/gv-trong-truong",
          defaults: new { controller = "Taikhoan", action = "Index", id = UrlParameter.Optional }
       );
            context.MapRoute(
              name: "qltkncs",
              url: "Admin/ql-tai-khoan/tai-khoan-ncs",
             defaults: new { controller = "Taikhoan", action = "NCS", id = UrlParameter.Optional }
          );
            context.MapRoute(
                 name: "qltintuc",
                 url: "Admin/ql-tin-tuc",
                defaults: new { controller = "TinTuc", action = "Index", id = UrlParameter.Optional }
             );
            context.MapRoute(
                 name: "qltinsua",
                 url: "Admin/ql-tin-tuc/sua-tin/{id}",
                defaults: new { controller = "TinTuc", action = "Edit", id = UrlParameter.Optional }
             );
            context.MapRoute(
                    name: "qltinthem",
                    url: "Admin/ql-tin-tuc/them-moi-tin",
                   defaults: new { controller = "TinTuc", action = "Create", id = UrlParameter.Optional }
                );
            context.MapRoute(
                name: "qlvb",
                url: "Admin/ql-van-ban",
               defaults: new { controller = "QuanLyVanBan", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                    name: "qlhethong",
                    url: "Admin/ql-he-thong/ql-phan-quyen",
                   defaults: new { controller = "Role", action = "Index", id = UrlParameter.Optional }
                );
            context.MapRoute(
               name: "qldmbaove",
               url: "Admin/ql-danh-muc-bao-ve",
              defaults: new { controller = "QuanLyDanhMucThongTin", action = "QuanLyDanhMuc", id = UrlParameter.Optional }
           );
            context.MapRoute(
                  name: "qltruongthongtin",
                  url: "Admin/ql-danh-muc-bao-ve/quan-ly-truong-thong-tin",
                 defaults: new { controller = "QuanLyDanhMucThongTin", action = "Index", id = UrlParameter.Optional }
              );
            context.MapRoute(
                 name: "qlbm",
                 url: "Admin/ql-bieu-mau",
                defaults: new { controller = "QuanLyBieuMau", action = "Index", id = UrlParameter.Optional }
             );
            context.MapRoute(
                name: "thembm",
                url: "Admin/ql-bieu-mau/them-moi-bieu-mau",
               defaults: new { controller = "QuanLyBieuMau", action = "Create", id = UrlParameter.Optional }
            );
            context.MapRoute(
                   name: "suabm",
                   url: "Admin/ql-bieu-mau/sua-bieu-mau/{id}",
                  defaults: new { controller = "QuanLyBieuMau", action = "Edit", id = UrlParameter.Optional }
                );
            context.MapRoute(
               name: "thamsobm",
               url: "Admin/ql-bieu-mau/ql-tham-so-bieu-mau",
              defaults: new { controller = "QuanLyThamSoBieuMau", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
                 name: "qllog",
                 url: "Admin/ql-log",
                defaults: new { controller = "SysLog", action = "Index", id = UrlParameter.Optional }
              );
            context.MapRoute(
             name: "qllogcrash",
             url: "Admin/ql-log/log-crash",
            defaults: new { controller = "SysLog", action = "Crash", id = UrlParameter.Optional }
            );
            context.MapRoute(
            name: "xoalog",
            url: "Admin/ql-log/xoa-log",
            defaults: new { controller = "SysLog", action = "RemoveLog", id = UrlParameter.Optional }
            );
            context.MapRoute(
             name: "cauhinhht",
             url: "Admin/cau-hinh-he-thong",
            defaults: new { controller = "SysSetting", action = "Index", id = UrlParameter.Optional }
            );
            context.MapRoute(
            name: "trangchudamin",
            url: "Admin/quan-tri-he-thong-qlncs",
           defaults: new { controller = "HomeAdmin", action = "Index", id = UrlParameter.Optional }
           );
            context.MapRoute(
           name: "quanlydotts",
           url: "Admin/quan-ly-dot-tuyen-sinh",
          defaults: new { controller = "QuanLyDotTuyenSinh", action = "Index", id = UrlParameter.Optional }
          );
            context.MapRoute(
           name: "guithongbaoxtb",
           url: "Admin/gui-thong-bao-xin-tieu-ban",
          defaults: new { controller = "ThiSinhTuyenSinh", action = "GuiThongBaoXTB", id = UrlParameter.Optional }
          );
            context.MapRoute(
           name: "thongkencs",
           url: "Admin/ncs/thong-ke-tuyen-sinh-nop-hs",
          defaults: new { controller = "HocVien", action = "ThongKeTuyenSinhNCS", id = UrlParameter.Optional }
          );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}