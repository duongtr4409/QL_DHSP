import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

export  const apiUrl = {
  danhsachbieumau: server.apiUrl + 'DanhMucBieuMau/GetListPaging',
  thembieumau: server.apiUrl + 'DanhMucBieuMau/Insert',
  suabieumau: server.apiUrl + 'DanhMucBieuMau/Update',
  xoabieumau: server.apiUrl + 'DanhMucBieuMau/Delete',
  chitietbieumau: server.apiUrl + 'DanhMucBieuMau/GetByID',
  getall: server.apiUrl + 'DanhMucBieuMau/GetAll',
};
const api = {
  DanhSachBieuMau: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachbieumau, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    })
  },
  ChiTietBieuMau: (param) => {
    return apiGetAuth(apiUrl.chitietbieumau, {
      ...param
    });
  },
  ThemBieuMau: (param) => {
    console.log(param, 'param');
    return apiPostAuth(apiUrl.thembieumau, {...param}, true);
  },
  SuaBieuMau: (param) => {
    return apiPostAuth(apiUrl.suabieumau, {
      ...param
    });
  },
  XoaBieuMau: (param) => {
    return apiPostAuth(apiUrl.xoabieumau, {
      ...param
    });
  },
  GetAll: () =>{
    return apiGetAuth(apiUrl.getall);
  }
};

export default api;