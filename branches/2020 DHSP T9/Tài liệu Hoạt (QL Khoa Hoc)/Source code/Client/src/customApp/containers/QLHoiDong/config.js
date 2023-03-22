import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

const apiUrl = {
  danhsachtrangthai: server.apiUrl + 'HoiDong/HoiDong_DanhSach',
  suatrangthai: server.apiUrl + 'HoiDong/HoiDong_ChinhSuaThongTinChiTiet',
  xoatrangthai: server.apiUrl + 'HoiDong/HoiDong_XoaThongTinHoiDong',
  chitiettrangthai: server.apiUrl + 'HoiDong/HoiDong_ChiTiet',
  danhsachdanhgia: server.apiUrl + 'HoiDong/HoiDong_DanhSachDanhGia',
  savedanhsachdanhgia: server.apiUrl + 'HoiDong/HoiDong_LuuDanhSachDanhGia',
  capquanly: server.apiUrl + "DanhMucCapDeTai/GetAllAndGroup"
};
const api = {
  DanhSachHoiDong: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachtrangthai, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    })
  },
  ChiTietHoiDong: (param) => {
    return apiGetAuth(apiUrl.chitiettrangthai, {
      ...param
    });
  },
  SuaHoiDong: (param) => {
    return apiPostAuth(apiUrl.suatrangthai, {
      ...param
    });
  },
  XoaHoiDong: (param) => {
    return apiPostAuth(apiUrl.xoatrangthai, {
      ...param
    });
  },
  DanhSachDanhGia: (param) => {
    return apiGetAuth(apiUrl.danhsachdanhgia, {
      ...param
    });
  },
  SaveDanhSachDanhGia: (param) => {
    return apiPostAuth(apiUrl.savedanhsachdanhgia, param);
  },
  DanhSachCapQuanLy: (param) => {
    return apiGetAuth(apiUrl.capquanly, {
      ...param
    });
  }
};

export default api;