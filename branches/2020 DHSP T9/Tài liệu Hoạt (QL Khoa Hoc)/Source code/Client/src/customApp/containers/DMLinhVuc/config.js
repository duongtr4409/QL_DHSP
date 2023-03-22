import { apiGetAuth, apiPostAuth } from "../../../api";
import server from "../../../settings";

const apiUrl = {
  danhsachlinhvuc: server.apiUrl + "DanhMucLinhVuc/GetAllAndGroup",
  getalllinhvuc: server.apiUrl + "DanhMucLinhVuc/GetAll",
  themlinhvuc: server.apiUrl + "DanhMucLinhVuc/insert",
  themnhieulinhvuc: server.apiUrl + "DanhMucLinhVuc/InsertMultil",
  sualinhvuc: server.apiUrl + "DanhMucLinhVuc/Update",
  xoalinhvuc: server.apiUrl + "DanhMucLinhVuc/delete",
  danhsachlinhvuctucore: server.apiUrl + "DataInCore/getCategories",
  getallgrouplinhvuc: server.apiUrl + "DanhMucLinhVuc/GetAllAndGroup"
};
const api = {
  danhSachLinhVuc: (param) => {
    return apiGetAuth(apiUrl.danhsachlinhvuc, {
      ...param,
    });
  },

  chiTietLinhVuc: (param) => {
    return apiGetAuth(apiUrl.chitietlinhvuc, {
      ...param,
    });
  },
  themLinhVuc: (param) => {
    return apiPostAuth(apiUrl.themlinhvuc, {
      ...param,
    });
  },
  themNhieuLinhVuc: (param) => {
    return apiPostAuth(apiUrl.themnhieulinhvuc, {
      ...param,
    });
  },
  suaLinhVuc: (param) => {
    return apiPostAuth(apiUrl.sualinhvuc, {
      ...param,
    });
  },
  xoaLinhVuc: (param) => {
    return apiPostAuth(apiUrl.xoalinhvuc, [...param]);
  },
  danhSachLinhVucTuCore: (param) => {
    return apiGetAuth(apiUrl.danhsachlinhvuctucore, {
      ...param,
    });
  },
  DanhSachLinhVucGroup: (param) => {
    return apiGetAuth(apiUrl.getallgrouplinhvuc, {
      ...param,
    });
  },
};

export default api;
