import { apiGetAuth, apiGetUser, apiPostAuth } from "../../../api";
import server from "../../../settings";
import { getDefaultPageSize } from "../../../helpers/utility";

const apiUrl = {
  danhsachKQNC: server.apiInOut + "PhanQuyen/GetListPaging",
};
const api = {
  danhSachKQNC: (param) => {
    return apiGetAuth(apiUrl.danhsachKQNC, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },
};

export default api;
