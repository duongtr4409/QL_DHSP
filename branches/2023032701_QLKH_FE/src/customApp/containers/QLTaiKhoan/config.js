import { apiGetAuth, apiPostAuth, apiGet, apiPost } from "../../../api";
import server from "../../../settings";
import { getDefaultPageSize } from "../../../helpers/utility";

export const apiUrl = {
  danhsachtaikhoan: server.apiUrl + "HeThongCanBo/GetListPaging",
  themtaikhoan: server.apiUrl + "hethongcanbo/insert",
  suataikhoan: server.apiUrl + "hethongcanbo/update",
  xoataikhoan: server.apiUrl + "hethongcanbo/delete",
  chitiettaikhoan: server.apiUrl + "hethongcanbo/getbyid",

  resetmatkhau: server.apiUrl + "HeThongNguoiDung/ResetPassword",
  danhsachcoquan: server.apiUrl + "PhanQuyen/PhanQuyen_DanhSachCoQuan",
};
const api = {
  DanhSachTaiKhoan: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachtaikhoan, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },

  ChiTietTaiKhoan: (param) => {
    return apiGetAuth(apiUrl.chitiettaikhoan, {
      ...param,
    });
  },
  ThemTaiKhoan: (param) => {
    return apiPostAuth(apiUrl.themtaikhoan, { ...param });
  },
  SuaTaiKhoan: (param) => {
    return apiPostAuth(apiUrl.suataikhoan, {
      ...param,
    });
  },
  XoaTaiKhoan: (param) => {
    return apiPostAuth(apiUrl.xoataikhoan, {
      ...param,
    });
  },
  ResetMatKhau: (param) => {
    return apiGetAuth(apiUrl.resetmatkhau, {
      ...param,
    });
  },
  danhSachCoQuan: (param) => {
    return apiGetAuth(apiUrl.danhsachcoquan, {
      ...param,
    });
  },
};

export default api;
