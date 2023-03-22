import { apiGetAuth, apiPostAuth } from "../../../api";
import server from "../../../settings";
import { formDataCaller } from "../../../helpers/formDataCaller";
const apiUrl = {
  danhsachthongbao: server.apiUrl + "QuanLyThongBao/ThongBao_DanhSach",
  themsuathongbao: server.apiUrl + "QuanLyThongBao/ThongBao_ChinhSuaThongTinChiTiet",
  xoathongbao: server.apiUrl + "QuanLyThongBao/ThongBao_XoaThongBao",
  getbyid: server.apiUrl + "QuanLyThongBao/ThongBao_ChiTiet",
  getthongbao: server.apiUrl + "QuanLyThongBao/ThongBao_DanhSachHienThi",
  tatthongbao: server.apiUrl + "QuanLyThongBao/ThongBao_TatThongBao",
  danhsachcanbo: server.apiUrl + "QuanLyThongBao/ThongBao_DanhSachCanBo",
  danhsachcanbotheocapquanly: server.apiUrl + "QuanLyThongBao/ThongBao_DanhSachCanBoTheoCapQuanLy",
  danhsachcapdetai: server.apiUrl + "DanhMucCapDeTai/GetAllAndGroup",
  themfiledinhkem: server.apiUrl + "DeXuatDeTai/ThemMoiFileDinhKem",
  xoafiledinhkem: server.apiUrl + "DeXuatDeTai/XoaFileDinhKem",
};
const api = {
  DanhSachThongBao: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachthongbao, {
      ...param,
    });
  },
  EditThongBao: (param = {}) => {
    return apiPostAuth(apiUrl.themsuathongbao, {
      ...param,
    });
  },
  XoaThongBao: (param = {}) => {
    return apiPostAuth(apiUrl.xoathongbao, {
      ...param,
    });
  },
  GetByID: (param = {}) => {
    return apiGetAuth(apiUrl.getbyid, {
      ...param,
    });
  },
  GetThongBao: (param = {}) => {
    return apiGetAuth(apiUrl.getthongbao, {
      ...param,
    });
  },
  TatThongBao: (param = {}) => {
    return apiPostAuth(apiUrl.tatthongbao, param);
  },
  danhSachTaiKhoan: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachcanbo, {
      ...param,
    });
  },
  danhSachCanBoTheoCapQuanLy: (param = {}) => {
    return apiGetAuth(apiUrl.danhsachcanbotheocapquanly, {
      ...param,
    });
  },
  danhSachCapDeTai: (param) => {
    return apiGetAuth(apiUrl.danhsachcapdetai, {
      ...param,
    });
  },
  themFileDinhKem: (data) => {
    return formDataCaller(apiUrl.themfiledinhkem, data);
  },
  xoaFileDinhKem: (param) => {
    return apiPostAuth(apiUrl.xoafiledinhkem, { ...param });
  },
};

export default api;
