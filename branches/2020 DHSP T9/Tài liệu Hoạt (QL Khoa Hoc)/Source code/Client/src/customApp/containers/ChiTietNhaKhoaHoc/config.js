import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';

export const apiUrl = {
  chitietnhakhoahoc: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_ChiTiet',
  addeditthongtinchitiet: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_ChinhSuaThongTinChiTiet',
  xoathongtinchitiet: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_XoaThongTinChiTiet',
  addedithoatdongkhoahoc: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_ChinhSuaHoatDongKhoaHoc',
  xoahoatdongkhoahoc: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_XoaHoatDongKhoaHoc',
  editthongtinnhakhoahoc: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_CapNhat',
  themmoifiledinhkem: server.apiUrl + 'DeXuatDeTai/ThemMoiFileDinhKem',
  xoafiledinhkem: server.apiUrl + 'DeXuatDeTai/XoaFileDinhKem',
  capnhatanhdaidien: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_CapNhat_AnhDaiDien',
  deletenhakhoahoc: server.apiUrl + 'LyLichKhoaHoc/NhaKhoaHoc_Delete',
  detaibycanboid: server.apiUrl + 'DeTai/GetDeTaiByCanBoID',
};
const api = {
  ChiTietNhaKhoaHoc: (param = {}) => {
    return apiGetAuth(apiUrl.chitietnhakhoahoc, {
      ...param
    })
  },
  XoaFileDinhKem: (param = {}) => {
    return apiPostAuth(apiUrl.xoafiledinhkem, {
      ...param
    })
  },
  XoaThongTinChiTiet: (param = {}) => {
    return apiPostAuth(apiUrl.xoathongtinchitiet, {
      ...param
    })
  },
  XoaHoatDongKhoaHoc: (param = {}) => {
    return apiPostAuth(apiUrl.xoahoatdongkhoahoc, {
      ...param
    })
  },
  EditThongTinNhaKhoaHoc: (param = {}) => {
    return apiPostAuth(apiUrl.editthongtinnhakhoahoc, {
      ...param
    })
  },
  DeleteNhaKhoaHoc: (param = {}) => {
    return apiPostAuth(apiUrl.deletenhakhoahoc, {
      ...param
    })
  },
  DeTaiByCanBoID: (param = {}) => {
    return apiGetAuth(apiUrl.detaibycanboid, {
      ...param
    })
  },

};

export default api;