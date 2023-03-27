import {apiGetAuth, apiPostAuth, apiGet, apiPost} from "../../../api";
import server from '../../../settings';
import { getDefaultPageSize } from "../../../helpers/utility";

const apiUrl = {
  danhsachchucvu: server.apiUrl + 'danhmucchucvu/getlistpaging',
  chitietchucvu: server.apiUrl + 'danhmucchucvu/getbyid',
  themchucvu: server.apiUrl + 'danhmucchucvu/insert',
  suachucvu: server.apiUrl + 'danhmucchucvu/update',
  xoachucvu: server.apiUrl + 'danhmucchucvu/delete',
};
const api = {
  DanhSachChucVu: (param) => {
    return apiGetAuth(apiUrl.danhsachchucvu,{
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    });
  },
  ChiTietChucVu: (param) => {
    return apiGetAuth(apiUrl.chitietchucvu, {
      ...param
    });
  },
  ThemChucVu: (param) => {
    return apiPostAuth(apiUrl.themchucvu, {
      ...param
    });
  },
  SuaChucVu: (param) => {
    return apiPostAuth(apiUrl.suachucvu, {
      ...param
    });
  },
  XoaChucVu: (param) => {
    return apiPostAuth(apiUrl.xoachucvu, {
      ...param
    });
  },
};

export default api;