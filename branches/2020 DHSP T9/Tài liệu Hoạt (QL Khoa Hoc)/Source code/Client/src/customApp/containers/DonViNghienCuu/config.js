import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

export  const apiUrl = {
  danhsachdonvi: server.apiUrl + 'DonViNghienCuu/DanhSachDonViNghienCuu',
  danhsachnhansudonvi: server.apiUrl + 'DonViNghienCuu/DanhSachNhanSuTaiDonVi',
  danhsachdexuat: server.apiUrl + 'DonViNghienCuu/DanhSachDeXuatDaGuiFilter',
};
const api = {
  DanhSachDonVi: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachdonvi, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    })
  },
  DanhSachNhanSuDonVi: (param)=>{
    return apiGetAuth(apiUrl.danhsachnhansudonvi, {...param})
  },
  DanhSachDeXuat: (param)=>{
    return apiGetAuth(apiUrl.danhsachdexuat, {...param})
  }
};

export default api;