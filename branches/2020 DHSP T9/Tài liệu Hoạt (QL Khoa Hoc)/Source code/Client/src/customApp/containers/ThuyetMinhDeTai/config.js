import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';

export const apiUrl = {
  danhsachthuyetminh: server.apiUrl + 'ThuyetMinhDeTai/DanhSachDeXuatThuyetMinh',
  danhsachcapdetai: server.apiUrl + 'DanhMucCapDeTai/GetAllAndGroup',
  themthuyetminh: server.apiUrl + 'ThuyetMinhDeTai/InsertThuyetMinh',
  editthuyetminh: server.apiUrl + 'ThuyetMinhDeTai/UpdateThuyetMinh',
  getbyid: server.apiUrl + 'ThuyetMinhDeTai/GetByID',
  deletethuyetminh: server.apiUrl + 'ThuyetMinhDeTai/Delete',
  danhsachallthuyetminh: server.apiUrl + 'ThuyetMinhDeTai/DanhSachAllThuyetMinhDeXuat',
  duyetthuyetminh: server.apiUrl + 'ThuyetMinhDeTai/DuyetThuyetMinh'
};
const api = {
  DanhSachThuyetMinh: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachthuyetminh, {...param})
  },
  DanhSachCapQuanLy: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachcapdetai, {...param})
  },
  DanhSachCanBo: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachcanbo, {...param})
  },
  GetByID: (param = {}) => {
    return apiGetAuth(apiUrl.getbyid, {...param})
  },
  DeleteThuyetMinh: (param) => {
    return apiPostAuth(apiUrl.deletethuyetminh, param)
  },
  GetAllDanhSachThuyetMinh: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachallthuyetminh, {...param})
  },
  DuyetThuyetMinh: (param = {}) => {
    return apiPostAuth(apiUrl.duyetthuyetminh, {...param})
  }
};

export default api;