import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';

export const apiUrl = {
  danhsachchucdanh: server.apiUrl + 'DataInCore/getTitles',
  danhsachchucvu: server.apiUrl + 'DataInCore/getPositions',
  danhsachhocvi: server.apiUrl + 'DataInCore/getDegrees',
  danhsachnhiemvu: server.apiUrl + 'DataInCore/getCategories',
  danhsachnhiemvu2: server.apiUrl + 'DataInCore/getCategories_relation',
  danhsachnhiemvukhoahoc: server.apiUrl + 'DataInCore/DSNhiemVuKhoaHoc',
  danhsachquydoinhiemvu: server.apiUrl + 'DataInCore/getConversions',
  danhsachphongban: server.apiUrl + 'DataInCore/getDepartments',
  danhsachallnhiemvu: server.apiUrl + 'DataInCore/getALLCategories',
  danhsachcanbo: server.apiUrl + 'DataInCore/getstave',
};
const api = {
  DanhSachChucDanh: () => {
    return apiGetAuth(apiUrl.danhsachchucdanh)
  },
  DanhSachChucVu: () => {
    return apiGetAuth(apiUrl.danhsachchucvu)
  },
  DanhSachHocVi: () => {
    return apiGetAuth(apiUrl.danhsachhocvi)
  },
  DanhSachNhiemVu: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachnhiemvu, {...param})//param parentId nullable
  },
  DanhSachNhiemVu2: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachnhiemvu2, {...param})//param parentId nullable
  },
  DanhSachNhiemVuKhoaHoc: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachnhiemvukhoahoc, {...param})//param parentId nullable
  },
  DanhSachNhiemVuQuyDoi: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachquydoinhiemvu, {...param})//param categoryId nullable
  },
  DanhSachPhongBan: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachphongban, {...param})//param type
  },
  DanhSachAllNhiemVu: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachallnhiemvu, {...param})//param type
  },
  DanhSachCanBo: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachcanbo, {...param})//param departmentid (int) getall = 0, keyword (string)
  },
};

export default api;