import asyncComponent from "../helpers/AsyncFunc";

const routes = [
  //Dash board
  {
    path: "",
    component: asyncComponent(() => import("./containers/Dashboard")),
  },
  //QLKH
  {
    path: "ly-lich-khoa-hoc",
    component: asyncComponent(() => import("./containers/ChiTietNhaKhoaHoc")),
  },
  {
    path: "dm-linh-vuc",
    component: asyncComponent(() => import("./containers/DMLinhVuc")),
  },
  {
    path: "dm-cap-de-tai",
    component: asyncComponent(() => import("./containers/DMCapDeTai")),
  },
  {
    path: "ht-tai-khoan",
    component: asyncComponent(() => import("./containers/QLTaiKhoan")),
  },
  {
    isDetail: true,
    path: "ql-de-xuat/chi-tiet/:id",
    component: asyncComponent(() => import("./containers/QLDeXuat/ChiTietDeXuat")),
  },
  {
    path: "ql-de-xuat",
    component: asyncComponent(() => import("./containers/QLDeXuat")),
  },
  {
    path: "ql-de-tai",
    component: asyncComponent(() => import("./containers/QLDeTai")),
  },
  {
    isDetail: true,
    path: "ql-de-tai/chi-tiet/:id",
    component: asyncComponent(() => import("./containers/QLDeTai/ChiTietDetaiModal")),
  },
  //Báo cáo
  {
    path: "bao-cao",
    component: asyncComponent(() => import("./containers/BaoCao")),
  },
  //Quản trị hệ thống
  {
    path: "ql-nha-khoa-hoc",
    component: asyncComponent(() => import("./containers/QLNhaKhoaHoc")),
  },
  {
    path: "chi-tiet-nha-khoa-hoc",
    component: asyncComponent(() => import("./containers/ChiTietNhaKhoaHoc")),
  },
  {
    path: "tham-so",
    component: asyncComponent(() => import("./containers/ThamSoHeThong")),
  },
  {
    path: "ql-bieu-mau",
    component: asyncComponent(() => import("./containers/QLBieuMau")),
  },
  {
    path: "chuc-vu",
    component: asyncComponent(() => import("./containers/DMChucVu")),
  },
  {
    path: "phan-quyen",
    component: asyncComponent(() => import("./containers/QLPhanQuyen")),
  },
  {
    path: "buoc-thuc-hien",
    component: asyncComponent(() => import("./containers/DMBuocThucHien")),
  },
  // {
  //   path: "dm-trang-thai",
  //   component: asyncComponent(() => import("./containers/DMTrangThai")),
  // },
  {
    path: "dm-loai-ket-qua",
    component: asyncComponent(() => import("./containers/DMLoaiKetQua")),
  },
  {
    path: "dm-loai-hinh-nghien-cuu",
    component: asyncComponent(() => import("./containers/DMLoaiHinhNghienCuu")),
  },
  {
    path: "don-vi-nghien-cuu",
    component: asyncComponent(() => import("./containers/DonViNghienCuu")),
  },
  {
    path: "ql-hoi-dong",
    component: asyncComponent(() => import("./containers/QLHoiDong")),
  },
  {
    path: "ql-thong-bao",
    component: asyncComponent(() => import("./containers/ThongBao/")),
  },
  {
    path: "thuyet-minh-de-tai",
    component: asyncComponent(() => import("./containers/ThuyetMinhDeTai")),
  },
  {
    isDetail: true,
    path: "thuyet-minh-de-tai/chi-tiet/:id",
    component: asyncComponent(() => import("./containers/ThuyetMinhDeTai/ChiTietDeXuatThuyetMinh")),
  },
];
export default routes;
