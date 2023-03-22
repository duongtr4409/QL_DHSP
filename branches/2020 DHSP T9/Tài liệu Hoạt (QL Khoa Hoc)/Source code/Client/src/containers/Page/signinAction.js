import {
  apiGet,
  apiPost
} from "../../../api";
import server from '../../../settings';

const apiUrl = {
  signin: server.apiUrl + 'Nguoidung/DangNhap',

};
const api = {
  dangNhap: (param) => {
    return apiGet(apiUrl.signin,{
      ...param
    });
  },
};

export default api;