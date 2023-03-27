import {apiGetAuth, apiPostAuth} from "../../../api";
import server from '../../../settings';
import {getDefaultPageSize} from "../../../helpers/utility";

const apiUrl = {
  danhsachbuocthuchien: server.apiUrl + 'DanhMucBuocThucHien/GetListPaging',
  thembuocthuchien: server.apiUrl + 'DanhMucBuocThucHien/Insert',
  suabuocthuchien: server.apiUrl + 'DanhMucBuocThucHien/Update',
  xoabuocthuchien: server.apiUrl + 'DanhMucBuocThucHien/Delete',
  chitietbuocthuchien: server.apiUrl + 'DanhMucBuocThucHien/GetByID',
  getall: server.apiUrl + 'DanhMucBuocThucHien/GetAll'
};
const api = {
  DanhSachBuocThucHien: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachbuocthuchien, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize()
    })
  },
  ChiTietBuocThucHien: (param) => {
    return apiGetAuth(apiUrl.chitietbuocthuchien, {
      ...param
    });
  },
  ThemBuocThucHien: (param) => {
    return apiPostAuth(apiUrl.thembuocthuchien, {...param});
  },
  SuaBuocThucHien: (param) => {
    return apiPostAuth(apiUrl.suabuocthuchien, {
      ...param
    });
  },
  XoaBuocThucHien: (param) => {
    return apiPostAuth(apiUrl.xoabuocthuchien, {
      ...param
    });
  },
  GetAll: () => {
    return apiGetAuth(apiUrl.getall);
  }
};

export default api;