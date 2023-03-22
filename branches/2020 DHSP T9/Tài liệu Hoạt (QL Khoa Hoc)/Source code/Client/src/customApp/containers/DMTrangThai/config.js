import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

const apiUrl = {
  danhsachtrangthai: server.apiUrl + 'DanhMucTrangThai/GetListPaging',
  themtrangthai: server.apiUrl + 'DanhMucTrangThai/Insert',
  suatrangthai: server.apiUrl + 'DanhMucTrangThai/Update',
  xoatrangthai: server.apiUrl + 'DanhMucTrangThai/Delete',
  chitiettrangthai: server.apiUrl + 'DanhMucTrangThai/GetByID',
  getall: server.apiUrl + 'DanhMucTrangThai/GetAll'
};
const api = {
  DanhSachTrangThai: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachtrangthai, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    })
  },
  ChiTietTrangThai: (param) => {
    return apiGetAuth(apiUrl.chitiettrangthai, {
      ...param
    });
  },
  ThemTrangThai: (param) => {
    return apiPostAuth(apiUrl.themtrangthai, {...param});
  },
  SuaTrangThai: (param) => {
    return apiPostAuth(apiUrl.suatrangthai, {
      ...param
    });
  },
  XoaTrangThai: (param) => {
    return apiPostAuth(apiUrl.xoatrangthai, {
      ...param
    });
  },
  GetAll: () => {
    return apiGetAuth(apiUrl.getall);
  }
};

export default api;