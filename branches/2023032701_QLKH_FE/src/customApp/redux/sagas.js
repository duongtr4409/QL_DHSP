import { all } from "redux-saga/effects";
import Dashboard from "./Dashboard/saga";
import NhacViec from "./NhacViec/saga";
import BaoCao from "./BaoCao/saga";
import QLTaiKhoan from "./QLTaiKhoan/saga";
import DMCapDeTai from "./DMCapDeTai/saga";
import DMChucVu from "./DMChucVu/saga";
import ThamSoHeThong from "./ThamSoHeThong/saga";
import QLPhanQuyen from "./QLPhanQuyen/saga";
import QLNhaKhoaHoc from "./QLNhaKhoaHoc/saga";
import QLBieuMau from "./QLBieuMau/saga";
import ChiTietNhaKhoaHoc from "./ChiTietNhaKhoaHoc/saga";
import DMBuocThucHien from "./DMBuocThucHien/saga";
import DMTrangThai from "./DMTrangThai/saga";
import DMLoaiKetQua from "./DMLoaiKetQua/saga";
import DMLoaiHinhNghienCuu from "./DMLoaiHinhNghienCuu/saga";
import QLDeXuat from "./QLDeXuat/saga";
import DonViNghienCuu from "./DonViNghienCuu/saga";
import DMLinhVuc from "./DMLinhVuc/saga";
import QLDeTai from "./QLDeTai/saga";
import QLHoiDong from "./QLHoiDong/saga";
import QLThongBao from "./QLThongBao/saga";
import ThuyetMinhDeTai from "./ThuyetMinhDeTai/saga";
export default function* devSaga() {
  yield all([
    Dashboard(),
    NhacViec(),
    BaoCao(),
    QLTaiKhoan(),
    DMCapDeTai(),
    DMChucVu(),
    ThamSoHeThong(),
    QLPhanQuyen(),
    QLNhaKhoaHoc(),
    QLBieuMau(),
    ChiTietNhaKhoaHoc(),
    DMBuocThucHien(),
    DMTrangThai(),
    DMLoaiKetQua(),
    DMLoaiHinhNghienCuu(),
    DonViNghienCuu(),
    QLDeXuat(),
    QLDeTai(),
    DMLinhVuc(),
    QLHoiDong(),
    QLThongBao(),
    ThuyetMinhDeTai(),
    //
  ]);
}
