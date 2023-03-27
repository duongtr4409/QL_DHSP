import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

export const apiUrl = {
  danhsachnhakhoahoc: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_DanhSach',
  themnhakhoahoc: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_ThemMoi'
};
const api = {
  DanhSachNhaKhoaHoc: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachnhakhoahoc, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : 12
    })
  }
};

export default api;