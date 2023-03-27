import { apiGetAuth, apiPostAuth } from "../../../api";
import server from "../../../settings";

const apiUrl = {
  danhsachcapdetai: server.apiUrl + "DanhMucCapDeTai/GetAllAndGroup",
  getallcapdetai: server.apiUrl + "DanhMucCapDeTai/GetAll",
  themcapdetai: server.apiUrl + "DanhMucCapDeTai/insert",
  themnhieucapdetai: server.apiUrl + "DanhMucCapDeTai/InsertMultil",
  suacapdetai: server.apiUrl + "DanhMucCapDeTai/Update",
  xoacapdetai: server.apiUrl + "DanhMucCapDeTai/delete",
  danhsachcapdetaitucore: server.apiUrl + "DataInCore/getCategories",
};
const api = {
  danhSachCapDeTai: (param) => {
    return apiGetAuth(apiUrl.danhsachcapdetai, {
      ...param,
    });
  },

  chiTietCapDeTai: (param) => {
    return apiGetAuth(apiUrl.chitietcapdetai, {
      ...param,
    });
  },
  themCapDeTai: (param) => {
    return apiPostAuth(apiUrl.themcapdetai, {
      ...param,
    });
  },
  themNhieuCapDeTai: (param) => {
    return apiPostAuth(apiUrl.themnhieucapdetai, {
      ...param,
    });
  },
  suaCapDeTai: (param) => {
    return apiPostAuth(apiUrl.suacapdetai, {
      ...param,
    });
  },
  xoaCapDeTai: (param) => {
    return apiPostAuth(apiUrl.xoacapdetai, [...param]);
  },
  danhSachCapDeTaiTuCore: (param) => {
    return apiGetAuth(apiUrl.danhsachcapdetaitucore, {
      ...param,
    });
  },
};

export default api;
