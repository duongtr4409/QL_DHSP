import { apiGetAuth, apiPostAuth } from "../../../api";
import server from "../../../settings";
import { getDefaultPageSize } from "../../../helpers/utility";

const apiUrl = {
  danhsachthamso: server.apiUrl + "SystemConfig/GetListPaging",
  chitietthamso: server.apiUrl + "SystemConfig/getbyid",
  themthamso: server.apiUrl + "SystemConfig/insert",
  suathamso: server.apiUrl + "SystemConfig/update",
  xoathamso: server.apiUrl + "SystemConfig/delete",
  getbykey: server.apiUrl + "SystemConfig/GetByKey",
};
const api = {
  DanhSachThamSo: (param) => {
    return apiGetAuth(apiUrl.danhsachthamso, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },
  ChiTietThamSo: (param) => {
    return apiGetAuth(apiUrl.chitietthamso, {
      ...param,
    });
  },
  ThemThamSo: (param) => {
    return apiPostAuth(apiUrl.themthamso, {
      ...param,
    });
  },
  SuaThamSo: (param) => {
    return apiPostAuth(apiUrl.suathamso, {
      ...param,
    });
  },
  XoaThamSo: (param) => {
    return apiPostAuth(apiUrl.xoathamso, {
      ...param,
    });
  },
  GetByKey: (param) => {
    return apiGetAuth(apiUrl.getbykey, {
      ...param,
    });
  },
};

export default api;
