import { apiGetAuth, apiGetUser, apiPostAuth } from "../../../api";
import server from "../../../settings";
import { getDefaultPageSize } from "../../../helpers/utility";

const apiUrl = {
  danhsachnhom: server.apiUrl + "PhanQuyen/GetListPaging",
  danhsachcanbo: server.apiUrl + "hethongcanbo/getlistpaging",
  themnhom: server.apiUrl + "PhanQuyen/NhomNguoiDung_Insert",
  suanhom: server.apiUrl + "PhanQuyen/NhomNguoiDung_Update",
  chitietnhom: server.apiUrl + "PhanQuyen/NhomNguoiDung_GetFoUpdate",
  sieuchitietnhom: server.apiUrl + "PhanQuyen/NhomNguoiDung_GetChiTietByNhomNguoiDungID",
  xoanhom: server.apiUrl + "PhanQuyen/NhomNguoiDung_Delete",
  //chitietnguoidung: server.apiUrl + 'HeThongNguoiDung/GetByIDForPhanQuyen',
  danhsachchucnangduocthaotac: server.apiUrl + "PhanQuyen/PhanQuyen_GetQuyenDuocThaoTacTrongNhom",
  danhsachnguoidung: server.apiUrl + "HeThongNguoiDung/HeThong_NguoiDung_GetListBy_NhomNguoiDungID",
  themnguoidung: server.apiUrl + "PhanQuyen/NguoiDung_NhomNguoiDung_Insert",
  xoanguoidung: server.apiUrl + "PhanQuyen/NguoiDung_NhomNguoiDung_Delete",
  themchucnang: server.apiUrl + "PhanQuyen/PhanQuyen_InsertMult",
  suachucnang: server.apiUrl + "PhanQuyen/PhanQuyen_Update",
  xoachucnang: server.apiUrl + "PhanQuyen/PhanQuyen_Delete",

  danhsachnhomnguoidungbycoquanid: server.apiUrl + "PhanQuyen/GetListNNDByCoQuanID",
  danhsachcoquan: server.apiUrl + "PhanQuyen/PhanQuyen_DanhSachCoQuan",
  danhsachnguoidungtheocoquan: server.apiUrl + "PhanQuyen/PhanQuyen_DSNguoiDungTheoCoQuan",
  danhsachnguoidungtheocoquanvanhomnguoidung: server.apiUrl + "PhanQuyen/PhanQuyen_DSNguoiDungTheoCoQuanVaNhomNguoiDungID",
};
const api = {
  danhSachNhom: (param) => {
    return apiGetAuth(apiUrl.danhsachnhom, {
      ...param,
      PageNumber: param.PageNumber ? param.PageNumber : 1,
      PageSize: param.PageSize ? param.PageSize : getDefaultPageSize(),
    });
  },
  danhSachCoQuan: (param) => {
    return apiGetAuth(apiUrl.danhsachcoquan, {
      ...param,
    });
  },
  danhSachCanBo: (param) => {
    return apiGetAuth(apiUrl.danhsachcanbo, {
      ...param,
      PageNumber: 1,
      PageSize: 999999,
    });
  },
  themNhom: (param) => {
    return apiPostAuth(apiUrl.themnhom, {
      ...param,
    });
  },
  suaNhom: (param) => {
    return apiPostAuth(apiUrl.suanhom, {
      ...param,
    });
  },
  chiTietNhom: (param) => {
    return apiGetAuth(apiUrl.chitietnhom, {
      ...param,
    });
  },
  sieuChiTietNhom: (param) => {
    return apiGetAuth(apiUrl.sieuchitietnhom, {
      ...param,
    });
  },
  xoaNhom: (param) => {
    return apiPostAuth(apiUrl.xoanhom, {
      ...param,
    });
  },
  // chiTietNguoiDung: (param) => {
  //   return apiGetUser(apiUrl.chitietnguoidung, {
  //     ...param
  //   });
  // },
  danhSachChucNangDuocThaoTac: (param) => {
    return apiGetAuth(apiUrl.danhsachchucnangduocthaotac, {
      ...param,
    });
  },
  danhSachNguoiDung: (param) => {
    return apiGetAuth(apiUrl.danhsachnguoidung, {
      ...param,
    });
  },
  themNguoiDung: (param) => {
    return apiPostAuth(apiUrl.themnguoidung, {
      ...param,
    });
  },
  xoaNguoiDung: (param) => {
    return apiPostAuth(apiUrl.xoanguoidung, {
      ...param,
    });
  },
  themChucNang: (paramArray) => {
    return apiPostAuth(apiUrl.themchucnang, paramArray);
  },
  suaChucNang: (paramArray) => {
    return apiPostAuth(apiUrl.suachucnang, paramArray);
  },
  xoaChucNang: (param) => {
    return apiPostAuth(apiUrl.xoachucnang, {
      ...param,
    });
  },
  DanhSachNhomByCoQuanID: (param) => {
    return apiGetAuth(apiUrl.danhsachnhomnguoidungbycoquanid, {
      ...param,
    });
  },
  danhSachNguoiDungTheoCoQuanID: (param) => {
    return apiGetAuth(apiUrl.danhsachnguoidungtheocoquan, {
      ...param,
    });
  },
  danhSachNguoiDungTheoCoQuanIDvaNhomNguoiDungID: (param) => {
    return apiGetAuth(apiUrl.danhsachnguoidungtheocoquanvanhomnguoidung, {
      ...param,
    });
  },
  // danhSachNguoiDungTheoCoQuanID: (param) => {
  //   return apiGetAuth(apiUrl.danhsachnguoidungtheocoquan, {
  //     ...param,
  //   });
  // },
};

export default api;
