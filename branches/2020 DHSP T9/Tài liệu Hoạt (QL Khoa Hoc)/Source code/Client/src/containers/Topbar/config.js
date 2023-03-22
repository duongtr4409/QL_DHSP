import { apiGetAuth, apiPostAuth } from "../../api";
import server from "../../settings";
import { getDefaultPageSize } from "../../helpers/utility";

const apiUrl = {
  getnotifications: server.apiUrl + "QuanLyThongBao/ThongBao_DanhSachHienThi",
  danhsachhuongdan: server.apiUrl + "HuongDanSuDung/GetAll",
  changepassword: server.apiUrl + "Hethongnguoidung/ChangePassword",
  updatenotifications: server.apiUrl + "NhacViec/Update",
};
const api = {
  getNotifications: (param) => {
    return apiGetAuth(apiUrl.getnotifications, { ...param, PageSize: 9000 });
  },
  updateNotifications: (param) => {
    return apiPostAuth(apiUrl.updatenotifications, { ...param });
  },
  danhSachHuongDan: (param) => {
    return apiGetAuth(apiUrl.danhsachhuongdan, {
      ...param,
      PageNumber: 1,
      PageSize: 1000,
    });
  },
  changePassword: (param) => {
    return apiPostAuth(apiUrl.changepassword, { ...param });
  },
};

export default api;
