import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

const apiUrl = {
  danhsachloaihinhnghiencuu: server.apiUrl + 'DanhMucLoaiHinhNghienCuu/GetListPaging',
  themloaihinhnghiencuu: server.apiUrl + 'DanhMucLoaiHinhNghienCuu/Insert',
  sualoaihinhnghiencuu: server.apiUrl + 'DanhMucLoaiHinhNghienCuu/Update',
  xoaloaihinhnghiencuu: server.apiUrl + 'DanhMucLoaiHinhNghienCuu/Delete',
  chitietloaihinhnghiencuu: server.apiUrl + 'DanhMucLoaiHinhNghienCuu/GetByID',
  getall: server.apiUrl + 'DanhMucLoaiHinhNghienCuu/GetAll'
};
const api = {
  DanhSachLoaiHinhNghienCuu: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachloaihinhnghiencuu, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    })
  },
  ChiTietLoaiHinhNghienCuu: (param) => {
    return apiGetAuth(apiUrl.chitietloaihinhnghiencuu, {
      ...param
    });
  },
  ThemLoaiHinhNghienCuu: (param) => {
    return apiPostAuth(apiUrl.themloaihinhnghiencuu, {...param});
  },
  SuaLoaiHinhNghienCuu: (param) => {
    return apiPostAuth(apiUrl.sualoaihinhnghiencuu, {
      ...param
    });
  },
  XoaLoaiHinhNghienCuu: (param) => {
    return apiPostAuth(apiUrl.xoaloaihinhnghiencuu, {
      ...param
    });
  },
  GetAll: () => {
    return apiGetAuth(apiUrl.getall);
  }
};

export default api;