import { apiGetAuth, apiPost, apiPostAuth } from "../../../api";
import server from "../../../settings";

const apiUrl = {
  dangnhapsso: server.apiUrl + "Nguoidung/DangNhapSSO",
  getbykey: server.apiUrl + "SystemConfig/GetByKey",
};
const api = {
  DangNhapSSO: (param) => {
    return apiPost(apiUrl.dangnhapsso, { ...param });
  },
  GetByKey: (param) => {
    return apiGetAuth(apiUrl.getbykey, {
      ...param,
    });
  },
};

export default api;
