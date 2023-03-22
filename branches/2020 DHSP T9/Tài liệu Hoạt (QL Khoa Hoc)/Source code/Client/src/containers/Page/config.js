import {
  apiGet,
  apiGetAuth,
  apiGetUser,
  apiPost
} from "../../api";
import server from '../../settings';

const apiUrl = {
  dangnhap: server.apiUrl + 'Nguoidung/DangNhap',
  chitiet: server.apiUrl + 'HeThongNguoiDung/GetByIDForPhanQuyen',
  getdataconfig: server.apiUrl + 'SystemConfig/GetByKey'
};
const api = {
  dangNhap: (param) => {
    return apiPost(apiUrl.dangnhap, {
      ...param
    });
  },
  chiTiet: (param) => {
    return apiGetUser(apiUrl.chitiet, {
      ...param
    });
  },
  getDataConfig : (param) => {
    return apiGet(apiUrl.getdataconfig, {...param});
  }
};

export default api;