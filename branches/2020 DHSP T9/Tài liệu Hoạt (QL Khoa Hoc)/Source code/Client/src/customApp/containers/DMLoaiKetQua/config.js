import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

const apiUrl = {
  danhsachloaiketqua: server.apiUrl + 'DanhMucLoaiKetQua/GetListPaging',
  danhsachloaiketquagroup: server.apiUrl + 'DanhMucLoaiKetQua/GetAllAndGroup',
  themloaiketqua: server.apiUrl + 'DanhMucLoaiKetQua/Insert',
  sualoaiketqua: server.apiUrl + 'DanhMucLoaiKetQua/Update',
  xoaloaiketqua: server.apiUrl + 'DanhMucLoaiKetQua/Delete',
  chitietloaiketqua: server.apiUrl + 'DanhMucLoaiKetQua/GetByID',
  getall: server.apiUrl + 'DanhMucLoaiKetQua/GetAll',
  themnhieuketqua: server.apiUrl + 'DanhMucLoaiKetQua/InsertMultil'
};
const api = {
  DanhSachLoaiKetQua: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachloaiketqua, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    })
  },
  DanhSachLoaiKetQuaGroup: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachloaiketquagroup, {
      ...param
    })
  },
  ChiTietLoaiKetQua: (param) => {
    return apiGetAuth(apiUrl.chitietloaiketqua, {
      ...param
    });
  },
  ThemLoaiKetQua: (param) => {
    return apiPostAuth(apiUrl.themloaiketqua, {...param});
  },
  ThemNhieuLoaiKetQua: (param) => {
    return apiPostAuth(apiUrl.themnhieuketqua, {...param});
  },
  SuaLoaiKetQua: (param) => {
    return apiPostAuth(apiUrl.sualoaiketqua, {
      ...param
    });
  },
  XoaLoaiKetQua: (param) => {
    return apiPostAuth(apiUrl.xoaloaiketqua, param);
  },
  GetAll: () => {
    return apiGetAuth(apiUrl.getall);
  }
};

export default api;